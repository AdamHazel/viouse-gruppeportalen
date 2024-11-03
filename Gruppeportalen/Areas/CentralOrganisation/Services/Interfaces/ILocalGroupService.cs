using Gruppeportalen.Areas.CentralOrganisation.Models;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;

public interface ILocalGroupService
{
    bool AddNewLocalGroup(LocalGroup localGroup, string organisationId);
    
    List<LocalGroup> GetLocalGroups(string organisationId);
}