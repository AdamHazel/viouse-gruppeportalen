using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace Gruppeportalen.Services.Classes;

public class LocalGroupAdminService : ILocalGroupAdminService
{
    private readonly ApplicationDbContext _db;
    private readonly IApplicationUserService _au;

    public LocalGroupAdminService(ApplicationDbContext db, IApplicationUserService au)
    {
        _db = db;
        _au = au;
    }
    
    private LocalGroup? _getLocalGroupById(Guid id)
    {
        return _db.LocalGroups
            .Include(g => g.LocalGroupAdmins)
            .FirstOrDefault(g => g.Id == id);
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

    private LocalGroupAdmin? _getLocalGroupAdmin(Guid groupId, string userId)
    {
        return _db.LocalGroupAdmins.FirstOrDefault(a => a.LocalGroupId == groupId && a.UserId == userId);
    }

    private bool _removeAdmin(ApplicationUser user, LocalGroup group)
    {
        try
        {
            var adminToRemove = _getLocalGroupAdmin(group.Id, user.Id);
            if (adminToRemove == null)
                throw new DbUpdateException("Did not find admin to remove");
            
            group.LocalGroupAdmins.Remove(adminToRemove);
            user.LocalGroupAdmins.Remove(adminToRemove);
            if (_db.SaveChanges() > 0)
                return true;
            else
                throw new DbUpdateException("Failed to remove admin from db");
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
            var pu = _au.GetPrivateUserByEmail(email.ToLower());
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

    public bool RemoveAdminById(string userId, Guid groupId)
    {
        bool result = false;

        var user = _au.GetPrivateUserById(userId);
        var localGroup = _getLocalGroupById(groupId);

        if (localGroup != null || user != null)
        {
            if (_removeAdmin(user, localGroup))
                result = true;
        }

        return result;
    }
    
    public List<(ApplicationUser, Guid)> GetLocalGroupAdminsByGroup(LocalGroup? group)
    {
        var list = new List<(ApplicationUser, Guid)>();
        
        if (group == null)
        {
            return list;
        }
        
        foreach (var record in group.LocalGroupAdmins)
        {
            var user = _au.GetPrivateUserById(record.UserId);
            if (user != null)
                list.Add((user, group.Id));
        }
        
        return list.OrderBy(u => u.Item1.Email).ToList(); 
    }
}