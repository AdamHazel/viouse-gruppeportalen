using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IOverviewService
{
    List<AdminLocalGroupOverview>? GetAdminLocalGroupOverview(string userId);
    List<PersonLocalGroupOverview>? GetPersonLocalGroupOverview(string userId);
    
    List<(ApplicationUser, Guid)> GetLocalGroupAdminsByGroup(LocalGroup? group);
}