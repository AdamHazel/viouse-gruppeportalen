using System.Text;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;


namespace Gruppeportalen.Services.Classes;

public class OverviewService : IOverviewService
{
    private readonly ILocalGroupService _lgs;
    private readonly IApplicationUserService _au;
    private readonly IPrivateUserOperations _puo;

    public OverviewService(ILocalGroupService lgs, IApplicationUserService au, IPrivateUserOperations puo)
    {
        _lgs = lgs;
        _au = au;
        _puo = puo;
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
        return overviews.OrderBy(lg => lg.LocalGroupName).ToList();
    }

    public List<PersonLocalGroupOverview>? GetPersonLocalGroupOverview(string userId)
    {
        var overviews = new List<PersonLocalGroupOverview>();
        var user = _puo.GetPrivateUserByIdWithConnectedPersons(userId);
        
        var connections = user.UserPersonConnections;
        if (!connections.IsNullOrEmpty())
        {
            var listOfGroups = new HashSet<LocalGroup>();
            foreach (var upc in connections)
            {
                var person = upc.Person;
                foreach (var membership in person.Memberships)
                {
                    if (membership.IsActive && membership.LocalGroup.Active)
                    {
                        listOfGroups.Add(membership.LocalGroup);
                    }
                }
            }

            foreach (var group in listOfGroups)
            {
                var overview = new PersonLocalGroupOverview
                {
                    LocalGroupId = group.Id,
                    LocalGroupName = group.GroupName,
                };
                overviews.Add(overview);
            }
        }
        
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