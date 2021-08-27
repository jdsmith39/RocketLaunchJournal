using RocketLaunchJournal.Model;
using System.ComponentModel.DataAnnotations;

namespace RocketLaunchJournal.Infrastructure.Dtos
{
    public class LaunchDto : LaunchBase
    {
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public bool IsUpdated { get; set; }

        public bool ShowDetails { get; set; }

        public string? Name { get; set; }
        public int UserId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is LaunchDto item)
            {
                return RocketId == item.RocketId && Altitude == item.Altitude && item.ApogeeToEjectionTime == ApogeeToEjectionTime
                    && item.AverageAcceleration == AverageAcceleration && item.BurnTime == BurnTime && item.CoastToApogeeTime == CoastToApogeeTime
                    && item.Date == Date && item.DescentSpeed == DescentSpeed && item.Duration == Duration
                    && item.EjectionAltitude == EjectionAltitude && item.IsActive == IsActive && item.LaunchId == LaunchId
                    && item.LaunchNumber == LaunchNumber && item.Motors == Motors && item.Name == Name && item.Note == Note
                    && item.PeakAcceleration == PeakAcceleration && item.TopSpeed == TopSpeed;
            }

            return false;
        }
    }
}
