using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class UserPersonConnectionsService : IUserPersonConnectionsService
{
    private readonly ApplicationDbContext _db;
    private readonly IPrivateUserOperations _puo;
    private readonly IPersonService _ps;

    public UserPersonConnectionsService(IPrivateUserOperations puo, IPersonService ps, ApplicationDbContext db)
    {
        _db = db;
        _puo = puo;
        _ps = ps;
    }

    private bool _addConnection(PrivateUser privateuser, Person person)
    {
        try
        {
            var upConnection = new UserPersonConnection { PrivateUserId = privateuser.Id, PersonId = person.Id };
            privateuser.UserPersonConnections.Add(upConnection);
            person.UserPersonConnections.Add(upConnection);
            if (_db.SaveChanges() > 0)
                return true;
            else
            {
                throw new DbUpdateException("Failed to add connection");
            }
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    
    public bool AddUserPersonConnection(string userId, string personId)
    {
        var privateUser = _puo.GetPrivateUser(userId);
        var person = _ps.GetPersonById(personId);

        if (privateUser is null || person is null)
        {
            return false;
        }
        
        return _addConnection(privateUser, person);
    }
}