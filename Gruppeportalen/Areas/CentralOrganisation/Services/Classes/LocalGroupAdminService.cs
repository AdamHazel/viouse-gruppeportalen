using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Classes;

public class LocalGroupAdminService : ILocalGroupAdminService
{
    private readonly ApplicationDbContext _db;

    public LocalGroupAdminService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    private ApplicationUser GetPrivateUserByEmail(string emailAddress)
    {
        var pu = _db.Users.FirstOrDefault(u => u.Email == emailAddress && u.TypeOfUser== Constants.Privateuser);
        return pu;
    }
    
    private bool _addAdminToDb(LocalGroupAdmin admin)
    {
        try
        {
            _db.LocalGroupAdmins.Add(admin);
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
            var pu = GetPrivateUserByEmail(email);
            if (pu != null)
            {
                var newAdmin = new LocalGroupAdmin { LocalGroupId = localGroupId, PrivateUserId = pu.Id };
                if (_addAdminToDb(newAdmin))
                    success = true;
            }
        }
        
        return success;
    }
}