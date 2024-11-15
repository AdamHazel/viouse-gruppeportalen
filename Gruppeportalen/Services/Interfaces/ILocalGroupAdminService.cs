using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface ILocalGroupAdminService
{
    bool AddAdminToLocalGroupByEmail(string email, Guid localGroupId);

    bool RemoveAdminById(string userId, Guid groupId);
    
    List<(ApplicationUser, Guid)> GetLocalGroupAdminsByGroup(LocalGroup? group);
    
}