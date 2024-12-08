using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class ApplicationUserService : IApplicationUserService
{
    private readonly ApplicationDbContext _db;

    public ApplicationUserService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public ApplicationUser? GetPrivateUserByEmail(string emailAddress)
    {
        var pu = _db.Users
            .Include(g => g.LocalGroupAdmins)
            .FirstOrDefault(u => u.Email == emailAddress && u.TypeOfUser== Constants.Privateuser);
        return pu;
    }

    public ApplicationUser? GetPrivateUserById(string userId)
    {
        var pu = _db.Users
            .Include(g => g.LocalGroupAdmins)
            .FirstOrDefault(u => u.Id == userId && u.TypeOfUser== Constants.Privateuser);
        return pu;
    }

    public bool DoesUserExist(string emailAddress)
    {
        return _db.Users.Any(u => u.UserName == emailAddress);
    }

    public bool IsUserPrivateUser(string emailAddress)
    {
        return _db.Users.Any(u => u.UserName == emailAddress && u.TypeOfUser== Constants.Privateuser);
    }
}