using RocketLaunchJournal.Infrastructure.UserIdentity;
using RocketLaunchJournal.Model;
using RocketLaunchJournal.Model.Adhoc;
using RocketLaunchJournal.Model.UserIdentity;
using System.Linq;

public static class UserPermissionServiceRoleRestrict
{
    public static IQueryable<Launch> RoleRestrictLaunches(this UserPermissionService ups, IQueryable<Launch> query, bool viewOnly)
    {
        if (ups.UserClaimModel.IsAdmin)
        { /* do nothing */ }
        else
        {
            if (!viewOnly)
                query = query.Where(w => w.Rocket!.UserId == ups.UserClaimModel.UserId);
        }

        return query;
    }

    public static IQueryable<Report> RoleRestrictReports(this UserPermissionService ups, IQueryable<Report> query, bool viewOnly)
    {
        if (!viewOnly)
            query = query.Where(w => w.UserId == ups.UserClaimModel.UserId);
        else
            query = query.Where(w => w.UserId == ups.UserClaimModel.UserId || w.IsShared);

        return query;
    }

    public static IQueryable<Rocket> RoleRestrictRockets(this UserPermissionService ups, IQueryable<Rocket> query, bool viewOnly)
    {
        if (ups.UserClaimModel.IsAdmin)
        { /* do nothing */ }
        else
        {
            if (!viewOnly)
                query = query.Where(w => w.UserId == ups.UserClaimModel.UserId);
        }

        return query;
    }

    public static IQueryable<Role> RoleRestrictRoles(this UserPermissionService ups, IQueryable<Role> query, bool grantingRoles, bool viewOnly)
    {
        if (grantingRoles && ups.UserClaimModel.RoleData != null && ups.UserClaimModel.RoleData.GrantableRoleIds != null)
            query = query.Where(w => ups.UserClaimModel.RoleData.GrantableRoleIds.Contains(w.RoleId));
        else if (grantingRoles || !viewOnly)
            query = query.Where(w => false);

        return query.OrderBy(o => o.Level);
    }

    public static IQueryable<User> RoleRestrictUsers(this UserPermissionService ups, IQueryable<User> query, bool viewOnly)
    {
        if (ups.UserClaimModel.IsAdmin)
        { /* do nothing */ }
        else
        {
            query = query.Where(w => w.UserId == ups.UserClaimModel.UserId); // They don't have access, filter them out.
        }

        return query;
    }
}
