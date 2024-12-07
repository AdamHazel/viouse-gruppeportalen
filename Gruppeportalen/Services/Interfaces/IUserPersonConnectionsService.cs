using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IUserPersonConnectionsService
{
    bool AddUserPersonConnection(string userId, string personId);
    
}