using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketLaunchJournal.Model
{
    public class LogTypeBase
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public Enums.LogTypeEnum LogTypeId { get; set; }

        [StringLength(Constants.FieldSizes.NameLength)]
        public string? Name { get; set; }
    }

    public class LogType : LogTypeBase
    {
        public virtual ICollection<SystemLog>? SystemLogs { get; set; }
    }
}
