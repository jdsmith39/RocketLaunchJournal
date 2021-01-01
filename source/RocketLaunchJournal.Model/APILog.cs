using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace RocketLaunchJournal.Model
{
    public class APILogBase
    {
        [Key]
        public long APILogId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string? RequestContentBlock { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string? ResponseContentBlock { get; set; }

        [Required, StringLength(200)]
        public string? TargetURL { get; set; }

        [Required]
        public DateTime TransmissionDateTime { get; set; }

        public DateTime? ResponseDateTime { get; set; }

        [Required]
        public bool IncomingRequest { get; set; }
    }

    public class APILog: APILogBase
    {

    }
}
