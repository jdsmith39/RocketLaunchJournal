using Microsoft.EntityFrameworkCore;
using RocketLaunchJournal.Entities;
using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model.Adhoc;
using RocketLaunchJournal.Model.UserIdentity;
using System.Linq;

public static class ReportQueries
{
    /// <summary>
    /// Get all reports the user has access to
    /// </summary>
    /// <param name="db">dataContext</param>
    /// <param name="ups">IUserPermissionService</param>
    /// <param name="viewOnly">if View only then entity change tracking is turned off</param>
    /// <returns>IQueryable of reports</returns>
    public static IQueryable<Report> RoleRestrictedReports(this DataContext db, UserPermissionService ups, bool viewOnly = true)
    {
        var query = ups.RoleRestrictReports(db.Reports.AsQueryable(), viewOnly);
        if (viewOnly)
            query = query.AsNoTracking();
        return query;
    }
}
