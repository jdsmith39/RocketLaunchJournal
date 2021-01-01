using RocketLaunchJournal.Model.Constants;
using RocketLaunchJournal.Model.UserIdentity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RocketLaunchJournal.Model
{
    public class SystemLogBase
    {
        [Key]
        public long LogId { get; set; }

        public int? UserId { get; set; }

        /// <summary>
        /// unrelated to anything, leave null.
        /// </summary>
        public Guid? LogGroupKey { get; set; }

        public Enums.LogTypeEnum LogTypeId { get; set; }

        [Required]
        public string? EventDescription { get; set; }

        [DataType(DataType.DateTime)]
        [Required]
        public DateTimeOffset EventDateTime { get; set; }

        /// <summary>
        /// IPAddress of the incoming request if available
        /// </summary>
        [StringLength(FieldSizes.IpAddressLength)]
        public string? IpAddress { get; set; }
    }

    public class SystemLog : SystemLogBase
    {
        #region Navigation Links

        public virtual User? User { get; set; }

        public virtual LogType? LogType { get; set; }

        #endregion
    }
}
