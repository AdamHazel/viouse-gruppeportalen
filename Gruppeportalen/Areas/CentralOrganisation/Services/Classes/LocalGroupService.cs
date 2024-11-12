using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.CentralOrganisation.Services.Classes;

class CustomException : Exception
{
    public CustomException(string message)
    {
    }
}

public class LocalGroupService : ILocalGroupService
{
    
    private readonly ApplicationDbContext _db;
    private readonly ILogger<LocalGroupService> _logger;
   
    
    public LocalGroupService(ApplicationDbContext db, ILogger<LocalGroupService> logger)
    {
        _db = db;
        _logger = logger;
    }

    private bool _addGroupToDb(LocalGroup lg)
    {
        try
        {
            _db.LocalGroups.Add(lg);
            if (_db.SaveChanges() > 0)
                return true;
            else
                throw new DbUpdateException("Failed to add group to db");
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Failed to add group to db");
            return false;
        }
    }

    private bool _updateLocalGroup(LocalGroup lg)
    {
        try
        {
            _db.LocalGroups.Update(lg);
            if (_db.SaveChanges() > 0)
                return true;
            else
                throw new DbUpdateException("Failed to update group in db");
        }
        catch (DbUpdateException)
        {
            _logger.LogError("Failed to add group to db");
            return false;
        }
    }
    
    public bool AddNewLocalGroup(LocalGroup localGroup, string organisationId)
    {
        localGroup.CentralOrganisationId = organisationId;
        return _addGroupToDb(localGroup);
    }

    public LocalGroup? GetLocalGroupById(Guid id)
    {
        return _db.LocalGroups
            .Include(g=>g.LocalGroupAdmins)
            .FirstOrDefault(g => g.Id == id);
    }

    public bool UpdateLocalGroup(LocalGroup lg)
    {
        var localGroup = GetLocalGroupById(lg.Id);
        if (localGroup == null)
            return false;
        else
        {
            localGroup.GroupName = lg.GroupName;
            localGroup.Address = lg.Address;
            localGroup.Postcode = lg.Postcode;
            localGroup.City = lg.City;
            localGroup.County = lg.County;
            return _updateLocalGroup(localGroup);
        }
    }
    public List<LocalGroup>? GetLocalGroups(string organisationId)
    {
        var groups = _db.LocalGroups
            .Where(g => g.CentralOrganisationId == organisationId)
            .OrderBy(g => g.GroupName)
            .ToList();
        return groups;
    }
}