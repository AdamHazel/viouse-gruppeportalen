using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;

public interface ILocalGroupAdminService
{
    bool AddAdminToLocalGroupByEmail(string email, Guid localGroupId);
    
}