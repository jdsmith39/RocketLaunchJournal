using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Dtos.Adhoc
{
    public class ReportSourceColumnDto
    {
        public string Name { get; set; } = default!;

        public string TypeName { get; set; } = default!;

        [Display(Name = "Output")]
        public bool InOutput { get; set; } = true;

        [Display(Name = "Sort Order")]
        public int? SortOrder { get; set; }

        public SortTypes? Sort { get; set; }

        public AggregateTypes Aggregate { get; set; }

        public string? FilterGroup1 { get; set; }

        public string? FilterGroup2 { get; set; }

        public string? FilterGroup3 { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is ReportSourceColumnDto item)
            {
                return Name == item.Name && item.TypeName == TypeName && item.InOutput == InOutput && SortOrder == item.SortOrder &&
                    item.Sort == Sort && Aggregate == item.Aggregate && FilterGroup1 == item.FilterGroup1 &&
                    FilterGroup2 == item.FilterGroup2 && FilterGroup3 == item.FilterGroup3;
            }

            return false;
        }
    }
}
