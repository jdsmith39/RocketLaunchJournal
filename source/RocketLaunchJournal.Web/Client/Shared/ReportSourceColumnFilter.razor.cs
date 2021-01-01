using Microsoft.AspNetCore.Components;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using SmartishTable;
using SmartishTable.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client.Shared
{
    public partial class ReportSourceColumnFilter : IFilter<ReportSourceColumnDto>
    {
        [CascadingParameter(Name = "SmartishTableRoot")]
        public Root<ReportSourceColumnDto> Root { get; set; }

        [Parameter]
        public List<ReportSourceColumnDto> Columns { get; set; }

        public Expression<Func<ReportSourceColumnDto, bool>> GetFilter()
        {
            if (Columns == null)
                return null;

            var nameList = Columns.Select(s => s.Name).ToList();

            return x => !nameList.Contains(x.Name);
        }

        protected override void OnInitialized()
        {
            Root.AddFilterComponent(this);
        }
    }
}
