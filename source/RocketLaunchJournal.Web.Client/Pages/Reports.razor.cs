using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Web.Client.Services;
using System.Text.Json;

namespace RocketLaunchJournal.Web.Client.Pages;

public partial class Reports : IDisposable
{
  [Inject]
  public IModalService ModalService { get; set; }

  [Inject]
  private AnonymousClient anonymousClient { get; set; }
  [Inject]
  private AuthorizedClient authorizedClient { get; set; }

  [CascadingParameter]
  private Task<AuthenticationState> authenticationStateTask { get; set; }

  private RocketLaunchJournal.Web.Shared.UserIdentity.UserClaimBuilder userInfo;

  private EditContext editContext;

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
  private ReportDataDto<JsonElement>? DataDto;

  internal List<ReportSourceColumnDto> sourceColumns = new List<ReportSourceColumnDto>();
  private ReportDto Dto = new ReportDto() { Columns = new List<ReportSourceColumnDto>() };
  private ReportDto DtoOriginal = default!;
  internal List<System.Reflection.PropertyInfo> dtoProperties = typeof(ReportDto).GetProperties().ToList();
  private List<SortTypes> sortTypes = SortTypes.Ascending.GetList();
  private List<int> sortOrders = new List<int>() { 1 };
  private List<AggregateTypes> otherAggregates = new[] { AggregateTypes.Count, AggregateTypes.GroupBy }.ToList();
  private List<AggregateTypes> numberAggregates = AggregateTypes.Average.GetList().OrderBy(o => o.GetDisplayName()).ToList();
  private List<AggregateTypes> dateAggregates = AggregateTypes.Average.GetList().Where(w => w != AggregateTypes.Sum && w != AggregateTypes.Average).OrderBy(o => o.GetDisplayName()).ToList();
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

    AddRemoveSortOrders();

    // must be last so event triggers after the rest is complete
    if (updatedReportSource)
      await UpdateReportSourceColumns();
    else
      await SelectedTable.Refresh();
  }

  private List<ReportSourceColumnDto> SourceColumns
  {
    get
    {
      return sourceColumns.Where(w => !Dto.Columns.Select(s => s.ColumnName).Any(ww => ww == w.ColumnName)).ToList();
    }
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
    var parameters = new Blazored.Modal.ModalParameters();
    parameters.Add(nameof(ShowReport.DataDtos), DataDto);
    parameters.Add(nameof(ShowReport.Dto), Dto);
    var modalReference = ModalService.Show<ShowReport>(Dto.Name, parameters, new ModalOptions() { });
    var modalResult = await modalReference.Result;
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

      await UpdateReportSourceColumns();
    }
  }

  private async Task UpdateReportSourceColumns()
  {
    var reportSource = reportSources!.Single(s => s.ReportSourceId == Dto.ReportSourceId);
    sourceColumns = await anonymousClient.GetReportSourceColumns(reportSource);
    // blazor does NOT know about the change in these kinds of events.
    await Task.WhenAll(SourceTable.Refresh(), SelectedTable.Refresh());
    StateHasChanged();
  }

  #region Sorting

  private Task SortOrderChanged(ReportSourceColumnDto item, int? newSortOrder)
  {
    var oldSortOrder = item.SortOrder;
    item.SortOrder = newSortOrder;
    if (oldSortOrder.HasValue && newSortOrder.HasValue)
    {
      // simple sort order swap
      var column = Dto.Columns.FirstOrDefault(w => w.Name != item.Name && w.SortOrder == newSortOrder);
      if (column is not null)
        column.SortOrder = oldSortOrder;
      else
        AddRemoveSortOrders();
    }
    else
    {
      if (!item.Sort.HasValue && item.SortOrder.HasValue)
        item.Sort = SortTypes.Ascending;
      else if (item.Sort.HasValue && !item.SortOrder.HasValue)
        item.Sort = null;

      AddRemoveSortOrders(item);
    }
    return Task.CompletedTask;
  }

  private Task SortChanged(ReportSourceColumnDto item, SortTypes? newSort)
  {
    var oldSort = item.Sort;
    if (newSort.HasValue && !item.SortOrder.HasValue)
      item.SortOrder = sortOrders.Last();
    else if (!newSort.HasValue && item.SortOrder.HasValue)
      item.SortOrder = null;

    item.Sort = newSort;

    if (oldSort.HasValue != newSort.HasValue)
      AddRemoveSortOrders();

    return Task.CompletedTask;
  }

  private void AddRemoveSortOrders(ReportSourceColumnDto itemUpdated = null)
  {
    var blah = 1;
    var query = Dto.Columns.Where(w => w.SortOrder.HasValue).OrderBy(o => o.SortOrder);
    if (itemUpdated is not null)
      query = query.ThenByDescending(o => o.Name == itemUpdated.Name);
    foreach (var item in query)
    {
      item.SortOrder = sortOrders[blah - 1];

      // adds the next one
      blah++;
      if (sortOrders.IndexOf(blah) == -1)
        sortOrders.Add(blah);
    }

    if (blah < sortOrders.Count)
    {
      for (int yackity = sortOrders.Count - 1; yackity >= blah; yackity--)
      {
        sortOrders.RemoveAt(yackity);
      }
    }
  }

  #endregion

  #region column navigation

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

  #endregion

  protected virtual void Dispose(bool disposing)
  {
    if (!disposedValue)
    {
      if (disposing)
      {
        // TODO: dispose managed state (managed objects)
        Dto.PropertyChanged -= ReportDefinitionDto_PropertyChanged;
      }

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
