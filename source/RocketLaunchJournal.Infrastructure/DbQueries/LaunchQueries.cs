using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model;
using System.Linq;

public static class LaunchQueries
{
    /// <summary>
    /// Get all restricted users
    /// </summary>
    /// <param name="db">dataContext</param>
    /// <param name="ups">IUserPermissionService</param>
    /// <param name="viewOnly">if View only then entity change tracking is turned off</param>
    /// <returns>IQueryable of Launch</returns>
    public static IQueryable<Launch> RoleRestrictedLaunches(this DataContext db, UserPermissionService ups, bool viewOnly = true)
    {
        var query = ups.RoleRestrictLaunches(db.Launches.AsQueryable(), viewOnly);
        if (viewOnly)
            query = query.AsNoTracking();
        return query;
    }

    /// <summary>
    /// Get all users based on filter criteria
    /// </summary>
    /// <param name="db">dataContext</param>
    /// <param name="ups">IUserPermissionService</param>
    /// <param name="activeOnly">if true, then return active only</param>
    /// <returns>IQueryable of Launch</returns>
    public static IQueryable<Launch> LaunchesByFilter(this DataContext db, UserPermissionService ups, bool activeOnly)
    {
        var query = db.RoleRestrictedLaunches(ups);
        if (activeOnly)
            query = query.Where(w => !w.AuditFields.InactiveDateTime.HasValue);
        return query;
    }
}
