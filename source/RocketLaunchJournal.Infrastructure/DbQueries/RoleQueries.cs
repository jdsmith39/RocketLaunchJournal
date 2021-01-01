using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.UserIdentity;
using System.Linq;

public static class RoleQueries
{
    /// <summary>
    /// Get all General role restricted roles
    /// </summary>
    /// <param name="db">dataContext</param>
    /// <param name="ups">IUserPermissionService</param>
    /// <param name="viewOnly">if View only then entity change tracking is turned off</param>
    /// <returns>IQueryable of Role</returns>
    public static IQueryable<Role> RoleRestrictedRoles(this DataContext db, UserPermissionService ups, bool grantingRoles, bool viewOnly = true)
    {
        var query = ups.RoleRestrictRoles(db.Roles.AsQueryable(), grantingRoles, viewOnly);
        if (viewOnly)
            query = query.AsNoTracking();
        return query;
    }
}
