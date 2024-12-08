using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;
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

    private UserPersonConnection? _getConnection(string userId, string personId)
    {
        return _db.UserPersonConnections.FirstOrDefault(upc => upc.PrivateUserId == userId && upc.PersonId == personId);
    }

    private bool _deleteConnection(PrivateUser privateuser, Person person)
    {
        try
        {
            var upConnection = _getConnection(privateuser.Id, person.Id);
            if (upConnection == null)
            {
                throw new DbUpdateException("Failed to delete connection");
            }
            privateuser.UserPersonConnections.Remove(upConnection);
            person.UserPersonConnections.Remove(upConnection);
            if (_db.SaveChanges() > 0)
                return true;
            else
            {
                throw new DbUpdateException("Failed to remove connection");
            }
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    
    public ResultOfOperation AddUserPersonConnection(string userId, string personId)
    {
        var privateUser = _puo.GetPrivateUserById(userId);
        var person = _ps.GetPersonById(personId);

        var result = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty,
        };

        if (privateUser is null || person is null)
        {
            result.Message = "Private or person is invalid";
        }
        else
        {
            result.Result = _addConnection(privateUser, person);
            if (!result.Result)
            {
                result.Message = "Failed to add connection";
            }
        }
        
        return result;
    }

    public ResultOfOperation? DeleteUserPersonConnection(string userId, string personId)
    {
        var privateUser = _puo.GetPrivateUserById(userId);
        var person = _ps.GetPersonById(personId);

        var result = new ResultOfOperation
        {
            Result = false,
            Message = string.Empty,
        };

        if (privateUser == null || person == null)
        {
            result.Message = "User or Person was null. Unable to complete operation";
            return result;
        }
        
        result.Result = _deleteConnection(privateUser, person);
        if (!result.Result)
        {
            result.Message = "Unable to delete connection";
            return result;
        }
        else
        {
            result.Result = true;
            return result;
        }
    }

    public bool DoesPersonHaveOtherConnections(string personId)
    {
        return _db.UserPersonConnections.Any(upConnection => upConnection.PersonId == personId);
    }
    
}