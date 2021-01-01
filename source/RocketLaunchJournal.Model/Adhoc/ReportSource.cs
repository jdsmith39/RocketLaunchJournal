using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketLaunchJournal.Model.Adhoc
{
    public class ReportSource
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int ReportSourceId { get; set; }

        [Required]
        [StringLength(Constants.FieldSizes.NameLength)]
        public string Name { get; set; } = default!;

        [Required]
        [StringLength(Constants.FieldSizes.NameLength)]
        public string SQLName { get; set; } = default!;
    }
}
