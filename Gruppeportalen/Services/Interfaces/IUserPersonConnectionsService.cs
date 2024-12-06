namespace Gruppeportalen.Services.Interfaces;

public interface IUserPersonConnectionsService
{
    bool AddUserPersonConnection(string userId, string personId);
}