using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Model.Adhoc
{
    public class ReportBase
    {
        [Key]
        public int ReportId { get; set; }

        [Display(Name = "Report Source")]
        [Required]
        public int ReportSourceId { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Display(Name = "Report Name")]
        [Required]
        [StringLength(Constants.FieldSizes.NameLength)]
        public string Name { get; set; }

        [Display(Name = "Share Report?")]
        public bool IsShared { get; set; }

        [Required]
        public string Data { get; set; }
    }

    public class Report : ReportBase
    {
        public OwnedTypes.AuditFields? AuditFields { get; set; }

        #region Navigation Links

        public virtual UserIdentity.User? User { get; set; }

        public virtual ReportSource ReportSource { get; set; }

        #endregion
    }
}
