using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;

public interface ILocalGroupAdminService
{
    bool AddAdminToLocalGroupByEmail(string email, Guid localGroupId);
    
    List<ApplicationUser> GetLocalGroupAdminsByGroup(LocalGroup? group);
    
}