using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;

namespace Gruppeportalen.Services.Interfaces;

public interface IUserPersonConnectionsService
{
    ResultOfOperation AddUserPersonConnection(string userId, string personId);
    ResultOfOperation? DeleteUserPersonConnection(string userId, string personId);
    bool DoesPersonHaveOtherConnections(string personId);
}