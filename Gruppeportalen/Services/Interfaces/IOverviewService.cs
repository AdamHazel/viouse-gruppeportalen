using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IOverviewService
{
    List<UserLocalGroupOverview>? GetUserLocalGroupOverview(string userId);
}