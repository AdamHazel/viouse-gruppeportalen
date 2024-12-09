using System.Text;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;


namespace Gruppeportalen.Services.Classes;

public class OverviewService : IOverviewService
{
    private readonly ILocalGroupService _lgs;
    private readonly IApplicationUserService _au;

    public OverviewService(ILocalGroupService lgs, IApplicationUserService au)
    {
        _lgs = lgs;
        _au = au;
    }
    
    public List<AdminLocalGroupOverview>? GetAdminLocalGroupOverview(string userId)
    {
        var overviews = new List<AdminLocalGroupOverview>();
        var user = _au.GetPrivateUserById(userId);

        if (user != null)
        {
            foreach (var admin in user.LocalGroupAdmins)
            {
                var overview = new AdminLocalGroupOverview();
                var group = _lgs.GetLocalGroupById(admin.LocalGroupId);
                if (group != null)
                {
                    overview.LocalGroupId = group.Id;
                    overview.LocalGroupName = group.GroupName;
                    overview.UserStatus = "Status: Admin";
                    overviews.Add(overview);
                }
            }
        }
        
        /*
         * More functionality to come:
         *      Need to check local groups again to find the groups that the user/person is a member in
         */
        return overviews.OrderBy(lg => lg.LocalGroupName).ToList();
    }
    
    public List<(ApplicationUser, Guid)> GetLocalGroupAdminsByGroup(LocalGroup? group)
    {
        var list = new List<(ApplicationUser, Guid)>();
        
        if (group == null)
        {
            return list;
        }
        
        foreach (var record in group.LocalGroupAdmins)
        {
            var user = _au.GetPrivateUserById(record.UserId);
            if (user != null)
                list.Add((user, group.Id));
        }
        
        return list.OrderBy(u => u.Item1.Email).ToList(); 
    }
}