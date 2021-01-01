using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RocketLaunchJournal.Model.Enums;
using System;
using RocketLaunchJournal.Infrastructure.Dtos.Helpers;

namespace RocketLaunchJournal.Infrastructure.Services.Rockets
{
    public class RocketsGet:BaseService
    {
        /// <summary>
        /// Gets all rockets that the logged in user has access to.
        /// </summary>
        /// <param name="activeOnly">active only items</param>
        public async Task<IEnumerable<RocketDto>> GetRockets(bool activeOnly)
        {
            return await (from x in db.RocketsByFilter(UserPermissionService, activeOnly)
                          let lastLaunch = db.Launches.OrderByDescending(o => o.Date).FirstOrDefault(w => w.RocketId == x.RocketId)
                          orderby x.Name
                          select new RocketDto()
                          {
                              UserId = x.UserId,
                              Name = x.Name,
                              Recovery = x.Recovery,
                              RocketId = x.RocketId,
                              Weight = x.Weight,
                              Diameter = x.Diameter,
                              Length = x.Length,
                              BlackPowderForApogee = x.BlackPowderForApogee,
                              BlackPowderForMain = x.BlackPowderForMain,
                              //NumberOfLaunches = lastLaunch != null ? lastLaunch.LaunchNumber : 0,
                              LastLaunchDate = lastLaunch != null ? lastLaunch.Date : (DateTime?)null,
                              IsActive = !x.AuditFields!.InactiveDateTime.HasValue,
                          }).ToListAsync();
        }

        /// <summary>
        /// Gets all rockets that the logged in user has access to.
        /// </summary>
        /// <param name="activeOnly">active only items</param>
        public async Task<IEnumerable<SelectOptionDto<int>>> GetRocketsForSelection()
        {
            return await (from x in db.RocketsByFilter(UserPermissionService, false)
                          orderby x.Name
                          select new SelectOptionDto<int>()
                          {
                              Value = x.RocketId,
                              Text = x.Name,
                          }).ToListAsync();
        }
    }
}
