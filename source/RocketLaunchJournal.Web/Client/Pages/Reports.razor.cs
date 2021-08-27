using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Web.Client.Services;
using RocketLaunchJournal.Web.Client.Shared.FormControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client.Pages
{
    public partial class Reports : IDisposable
    {
        [Inject]
        private AnonymousClient anonymousClient { get; set; }
        [Inject]
        private AuthorizedClient authorizedClient { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        private RocketLaunchJournal.Web.Shared.UserIdentity.UserClaimBuilder userInfo;

        private EditContext editContext;

        private EditCheckbox sharedComponent;

        private SmartishTable.Root<ReportSourceColumnDto> SourceTable;
        private SmartishTable.Root<ReportSourceColumnDto> SelectedTable;
        private List<ReportSourceDto>? reportSources;
        private List<ReportDto>? reports;
        private int selectedReportId;
        private int SelectedReportId 
        {
            get
                {
                return selectedReportId;
            }
            set
            {
                selectedReportId = value;
                SetReport();
            }
        }

        private bool showSharedReports = true;
        private bool ShowSharedReports
        {
            get { return showSharedReports; }
            set
            {
                showSharedReports = value;
                SetReport();
            }
        }

        private int defaultReportSourceId;
        private ReportDataDto DataDto;

        internal List<ReportSourceColumnDto> sourceColumns = new List<ReportSourceColumnDto>();
        private ReportDto Dto = new ReportDto() { Columns = new List<ReportSourceColumnDto>() };
        private ReportDto DtoOriginal = default!;
        internal List<System.Reflection.PropertyInfo> dtoProperties = typeof(ReportDto).GetProperties().ToList();
        private List<SortTypes> sortTypes = SortTypes.Ascending.GetList();
        private List<int> sortOrders = new List<int>() { 1 };
        private List<AggregateTypes> aggregates = AggregateTypes.Average.GetList().OrderBy(o=>o.GetDisplayName()).ToList();
        private string outputIdFormat = "output{0}";
        private string sortIdFormat = "sort{0}";
        private string sortOrderIdFormat = "sortOrder{0}";
        private string aggregateIdFormat = "aggregate{0}";

        private bool disposedValue;

        protected override async Task OnInitializedAsync()
        {
            editContext = new EditContext(Dto);

            await authenticationStateTask;
            userInfo = new RocketLaunchJournal.Web.Shared.UserIdentity.UserClaimBuilder(authenticationStateTask.Result.User);

            var tasks = new List<Task>();
            var reportSourcesTask = anonymousClient.GetReportSources();
            var reportsTask = userInfo.IsAuthenticated ? authorizedClient.GetReports() : anonymousClient.GetReports();
            await Task.WhenAll(reportSourcesTask, reportsTask);

            reportSources = reportSourcesTask.Result;
            reports = reportsTask.Result;

            defaultReportSourceId = reportSources!.First().ReportSourceId;

            Dto.PropertyChanged += ReportDefinitionDto_PropertyChanged;

            SetReport();
        }

        private async void SetReport()
        {
            if (SelectedReportId == 0)
                DtoOriginal = new ReportDto() { ReportSourceId = defaultReportSourceId, Columns = new List<ReportSourceColumnDto>() };
            else
                DtoOriginal = reports!.Single(w => w.ReportId == SelectedReportId);

            var updatedReportSource = Dto.ReportSourceId != DtoOriginal.ReportSourceId;
            Dto.Update(DtoOriginal);
            
            // must be last so event triggers after the rest is complete
            if (updatedReportSource)
                await UpdateReportSourceColumns(); 
            else
                StateHasChanged();
        }

        private bool dataUpdated
        {
            get
            {
                return !Dto.Equals(DtoOriginal);
            }
        }

        private async Task GenerateReport()
        {
            if (!editContext.Validate())
                return;

            DataDto = await anonymousClient.GenerateReport(Dto);
        }

        private async Task DownloadReport()
        {
            if (!editContext.Validate())
                return;

            await anonymousClient.DownloadReport(Dto);
        }

        private async Task Save()
        {
            if (!editContext.Validate())
                return;

            Dto.PropertyChanged -= ReportDefinitionDto_PropertyChanged;
            var isNew = Dto.ReportId == 0;
            Dto.IsUpdated = dataUpdated;
            Dto = (await authorizedClient.SaveReport(Dto)) ?? Dto;
            Dto.PropertyChanged += ReportDefinitionDto_PropertyChanged;
            UpdateReportList(isNew);
        }

        private async Task SaveAs()
        {
            if (!editContext.Validate())
                return;

            Dto.PropertyChanged -= ReportDefinitionDto_PropertyChanged;
            Dto.ReportId = 0;
            Dto.UserId = 0;
            Dto.IsUpdated = dataUpdated;
            Dto = (await authorizedClient.SaveReport(Dto)) ?? Dto;
            Dto.PropertyChanged += ReportDefinitionDto_PropertyChanged;
            UpdateReportList(true);
        }

        private void UpdateReportList(bool isNew)
        {
            if (Dto.ReportId == 0)
                return;

            if (isNew)
                reports!.Add(Dto);
            else
            {
                var index = reports!.FindIndex(w => w.ReportId == Dto.ReportId);
                reports[index].Update(Dto);
            }

            SelectedReportId = Dto.ReportId;
            SetReport();
        }

        private async Task Delete()
        {
            if (Dto.ReportId <= 0)
                return;

            var id = await authorizedClient.DeleteReport(Dto.ReportId);
            if (!id.HasValue || reports == null)
                return;

            reports.Remove(Dto);
            SelectedReportId = 0;
        }

        private IEnumerable<ReportDto> GetReports()
        {
            if (reports == null)
                return new List<ReportDto>();

            if (showSharedReports)
                return reports;
            else
                return reports.Where(w => w.UserId == userInfo.UserId);
        }

        private async void ReportDefinitionDto_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ReportDto.ReportSourceId))
            {
                if (Dto.ReportSourceId == 0)
                    return;

                if (Dto.ReportId > 0)
                    SelectedReportId = 0;

                await UpdateReportSourceColumns();            }
        }

        private async Task UpdateReportSourceColumns()
        {
            var reportSource = reportSources!.Single(s => s.ReportSourceId == Dto.ReportSourceId);
            sourceColumns = await anonymousClient.GetReportSourceColumns(reportSource);
            // blazor does NOT know about the change in these kinds of events.
            StateHasChanged();
        }

        private Task AddColumn(ReportSourceColumnDto item, int index)
        {
            if (Dto.Columns == null)
                Dto.Columns = new List<ReportSourceColumnDto>();

            SelectedTable.Add(item);

            return Task.CompletedTask;
        }

        private Task RemoveColumn(ReportSourceColumnDto item, int index)
        {
            SelectedTable!.RemoveAt(index);

            return Task.CompletedTask;
        }

        private Task MoveUp(ReportSourceColumnDto item, int index)
        {
            if (index == 0)
                return Task.CompletedTask;

            SelectedTable.UpdateAt(index, SelectedTable.GetAt(index - 1));
            SelectedTable.UpdateAt(index - 1, item);
            return Task.CompletedTask;
        }

        private Task MoveDown(ReportSourceColumnDto item, int index)
        {
            if (index == SelectedTable.SafeList.Count - 1)
                return Task.CompletedTask;

            var temp = SelectedTable.GetAt(index + 1);
            SelectedTable.UpdateAt(index + 1, item);
            SelectedTable.UpdateAt(index, temp);
            return Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                    Dto.PropertyChanged -= ReportDefinitionDto_PropertyChanged;
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
