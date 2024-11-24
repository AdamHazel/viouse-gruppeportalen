using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface ILocalGroupService
{
  
    bool AddNewLocalGroup(LocalGroup localGroup, string organisationId);
    LocalGroup? GetLocalGroupById(Guid id);

    bool UpdateLocalGroup(LocalGroup localGroup);
    
    bool UpdateLocalGroupAsAdmin(LocalGroup localGroup);
    
    List<LocalGroup>? GetLocalGroups(string organisationId);
    
}