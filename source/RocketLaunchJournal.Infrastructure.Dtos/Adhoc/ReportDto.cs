using RocketLaunchJournal.Model.Adhoc;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace RocketLaunchJournal.Infrastructure.Dtos.Adhoc
{
    public class ReportDto
    {
        public bool IsUpdated { get; set; }

        public int ReportId { get; set; }

        private int reportSourceId;
        [Display(Name = "Report Source")]
        [Required]
        public int ReportSourceId
        {
            get { return reportSourceId; }
            set { SetProperty(ref reportSourceId, value); }
        }

        public int? UserId { get; set; }

        [Display(Name = "Report Name")]
        [Required]
        [StringLength(Model.Constants.FieldSizes.NameLength)]
        public string Name { get; set; }

        [Display(Name = "Share Report?")]
        public bool IsShared { get; set; }

        [Display(Name="Columns")]
        [MinLength(1)]
        public List<ReportSourceColumnDto> Columns { get; set; }

        public string? Data { get; set; }

        public string? UserName { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ReportDto item)
            {
                var areBasicParamsSame = ReportId == item.ReportId && Name == item.Name && IsShared == item.IsShared && item.ReportSourceId == ReportSourceId;
                if (!areBasicParamsSame)
                    return false;

                var thisColumnsNull = Columns == null;
                var itemColumnsNull = item.Columns == null;

                if (thisColumnsNull && itemColumnsNull)
                    return true;

                if (thisColumnsNull && item.Columns != null)
                    return false;

                if (Columns != null && itemColumnsNull)
                    return false;

                if (Columns!.Count != item.Columns!.Count)
                    return false;

                for (int blah = 0; blah < Columns.Count; blah++)
                {
                    if (!Columns[blah].Equals(item.Columns[blah]))
                        return false;
                }

                return true;
            }

            return false;
        }

        protected bool SetProperty<T>(ref T prop, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(prop, value)) return false;
            prop = value;
            this.RaisePropertyChange(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public async void RaisePropertyChange(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public void Update(ReportDto dto, bool updateReportSource = true)
        {
            var columns = dto.Columns != null ? dto.Columns.SerializeJson().DeserializeJson<List<ReportSourceColumnDto>>()! : dto.Data!.DeserializeJson<List<ReportSourceColumnDto>>()!;

            Columns.Clear();
            Columns.AddRange(columns);
            IsShared = dto.IsShared;
            Name = dto.Name;
            ReportId = dto.ReportId;
            UserId = dto.UserId;
            UserName = dto.UserName;
            
            if (updateReportSource)
                reportSourceId = dto.ReportSourceId;
        }
    }
}
