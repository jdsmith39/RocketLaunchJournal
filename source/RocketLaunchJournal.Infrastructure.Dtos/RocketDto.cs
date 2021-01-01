using RocketLaunchJournal.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RocketLaunchJournal.Infrastructure.Dtos
{
    public class RocketDto : RocketBase
    {
        [Display(Name="(oz)", ShortName = "WT")]
        public decimal? WeightInOunces
        {
            get
            {
                if (!Weight.HasValue)
                    return null;
                return Math.Round(Weight.Value * .03527396m, 2, MidpointRounding.AwayFromZero);
            }
        }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public bool IsUpdated { get; set; }

        [Display(Name = "# of Launches")]
        public int NumberOfLaunches { get; set; }

        [Display(Name = "Last Launch Date", ShortName = "LLD")]
        public DateTime? LastLaunchDate { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj is RocketDto item)
            {
                return BlackPowderForApogee == item.BlackPowderForApogee && BlackPowderForMain == item.BlackPowderForMain
                    && Diameter == item.Diameter && item.Length == Length && Name == item.Name && IsActive == item.IsActive
                    && (item.Recovery ?? "") == (Recovery ?? "") && item.Weight == Weight && RocketId == item.RocketId;
            }

            return false;
        }

        public bool Contains(string search)
        {
            if (string.IsNullOrEmpty(search)) return true;

            if (Name == null) return false;

            return Name.Contains(search, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
