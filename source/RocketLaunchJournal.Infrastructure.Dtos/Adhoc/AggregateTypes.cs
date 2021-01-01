using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Infrastructure.Dtos.Adhoc
{
    public enum AggregateTypes : byte
    {
        [Display(Name = "Group By")]
        GroupBy = 0,
        Min = 1,
        Max = 2,
        Count = 3,
        Sum = 4,
        [Display(ShortName = "Avg")]
        Average = 5
    }
}
