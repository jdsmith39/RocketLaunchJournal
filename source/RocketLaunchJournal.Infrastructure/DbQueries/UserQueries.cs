using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using System.Linq;

public static class UserQueries
{
    /// <summary>
    /// Get all restricted users
    /// </summary>
    /// <param name="db">dataContext</param>
    /// <param name="ups">IUserPermissionService</param>
    /// <param name="viewOnly">if View only then entity change tracking is turned off</param>
    /// <returns>IQueryable of User</returns>
    public static IQueryable<User> RoleRestrictedUsers(this DataContext db, UserPermissionService ups, bool viewOnly = true)
    {
        var query = ups.RoleRestrictUsers(db.Users.AsQueryable(), viewOnly);
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
    /// <returns>IQueryable of User</returns>
    public static IQueryable<User> UsersByFilter(this DataContext db, UserPermissionService ups, bool activeOnly)
    {
        var query = db.RoleRestrictedUsers(ups);
        if (activeOnly)
            query = query.Where(w => !w.AuditFields.InactiveDateTime.HasValue);
        return query;
    }
}
