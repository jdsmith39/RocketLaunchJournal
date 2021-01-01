using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RocketLaunchJournal.Model.OwnedTypes
{
    [Owned]
    public class AuditFields
    {
        public AuditFields()
        { }

        public AuditFields(int userId, DateTime timestamp)
        {
            CreatedById = userId;
            CreatedDateTime = timestamp;
            UpdatedById = userId;
            UpdatedDateTime = timestamp;
        }

        [Display(Name = "Inactivated By Id")]
        [Column(nameof(InactivatedById))]
        public int? InactivatedById { get; set; }
        [Column(nameof(InactiveDateTime))]
        public DateTimeOffset? InactiveDateTime { get; set; }

        [Display(Name = "Created By Id")]
        [Column(nameof(CreatedById))]
        public int CreatedById { get; set; }
        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [Column(nameof(CreatedDateTime))]
        public DateTimeOffset CreatedDateTime { get; set; }

        [Display(Name = "Updated By Id")]
        [Column(nameof(UpdatedById))]
        public int UpdatedById { get; set; }
        [Display(Name = "Updated Date")]
        [DataType(DataType.DateTime)]
        [Column(nameof(UpdatedDateTime))]
        public DateTimeOffset UpdatedDateTime { get; set; }

        public void SetUpdated(int userId, DateTime timestamp)
        {
            UpdatedById = userId;
            UpdatedDateTime = timestamp;
        }

        public void SetActiveInactive(bool isActive, int userId, DateTime timestamp)
        {
            if (!isActive && !InactiveDateTime.HasValue)
            {
                InactiveDateTime = timestamp;
                InactivatedById = userId;
            }
            else if (isActive && InactiveDateTime.HasValue)
            {
                InactiveDateTime = null;
                InactivatedById = null;
            }
        }
    }
}
