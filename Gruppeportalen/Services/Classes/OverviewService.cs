using System.Text;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

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
    
    public List<UserLocalGroupOverview>? GetUserLocalGroupOverview(string userId)
    {
        var overviews = new List<UserLocalGroupOverview>();
        var user = _au.GetPrivateUserById(userId);

        if (user != null)
        {
            foreach (var admin in user.LocalGroupAdmins)
            {
                var overview = new UserLocalGroupOverview();
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
}