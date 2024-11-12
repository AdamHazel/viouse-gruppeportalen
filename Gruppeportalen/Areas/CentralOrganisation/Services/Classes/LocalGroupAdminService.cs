using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Classes;

public class LocalGroupAdminService : ILocalGroupAdminService
{
    private readonly ApplicationDbContext _db;

    public LocalGroupAdminService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    private ApplicationUser? _getPrivateUserByEmail(string emailAddress)
    {
        var pu = _db.Users
            .FirstOrDefault(u => u.Email == emailAddress && u.TypeOfUser== Constants.Privateuser);
        return pu;
    }
    
    private ApplicationUser? _getPrivateUserById(string userId)
    {
        var pu = _db.Users
            .Include(g => g.LocalGroupAdmins)
            .FirstOrDefault(u => u.Id == userId && u.TypeOfUser== Constants.Privateuser);
        return pu;
    }
    
    private LocalGroup? _getLocalGroupById(Guid id)
    {
        return _db.LocalGroups.FirstOrDefault(g => g.Id == id);
    }
    
    private bool _addAdminToDb(LocalGroupAdmin admin, LocalGroup group, ApplicationUser user)
    {
        try
        {
            group.LocalGroupAdmins.Add(admin);
            user.LocalGroupAdmins.Add(admin);
            if (_db.SaveChanges() > 0)
                return true;
            else
                throw new DbUpdateException("Failed to add group to db");
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    
    public bool AddAdminToLocalGroupByEmail(string email, Guid localGroupId)
    {
        bool success = false;

        if (localGroupId != Guid.Empty)
        {
            var pu = _getPrivateUserByEmail(email.ToLower());
            var lg = _getLocalGroupById(localGroupId);
            
            if (pu != null && lg != null)
            {
                var newAdmin = new LocalGroupAdmin { LocalGroupId = lg.Id, UserId = pu.Id };
                if (_addAdminToDb(newAdmin, lg, pu))
                {
                    success = true;
                }
            }
        }
        
        return success;
    }
    
    public List<ApplicationUser> GetLocalGroupAdminsByGroup(LocalGroup? group)
    {
        var list = new List<ApplicationUser>();
        
        if (group == null)
        {
            return list;
        }
        
        foreach (var record in group.LocalGroupAdmins)
        {
            var user = _getPrivateUserById(record.UserId);
            if (user != null)
                list.Add(user);
        }
        
        return list.OrderBy(u => u.Email).ToList(); 
    }
}