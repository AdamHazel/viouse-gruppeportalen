using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

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
    private readonly ICentralOrganisationService _cos;
   
    
    public LocalGroupService(ApplicationDbContext db, ILogger<LocalGroupService> logger,
        ICentralOrganisationService cos)
    {
        _db = db;
        _logger = logger;
        _cos = cos;
    }

    private bool _addGroupToDb(LocalGroup lg, string orgId)
    {
        try
        {
            var org = _cos.GetCentralOrganisationByUser(orgId);
            if (org == null)
            {
                throw new DbUpdateException("Failed to add group to db");
            }
            lg.CentralOrganisationId = orgId;
            
            _db.LocalGroups.Add(lg);
            org.LocalGroups.Add(lg);
            
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
        return _addGroupToDb(localGroup, organisationId);
    }

    public LocalGroup? GetLocalGroupById(Guid id)
    {
        var group = _db.LocalGroups
            .Include(g=>g.LocalGroupAdmins)
            .Include(g=>g.MembershipTypes)
            .FirstOrDefault(g => g.Id == id);

        if (group != null)
        {
            group.MembershipTypes = group.MembershipTypes.OrderBy(m => m.MembershipName).ToList();
        }

        return group;
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

    public bool UpdateLocalGroupAsAdmin(LocalGroup lg)
    {
        var localGroup = GetLocalGroupById(lg.Id);
        if (localGroup == null)
            return false;
        
        else {
            localGroup.Active= lg.Active;
            localGroup.GroupName = lg.GroupName;
            localGroup.Address = lg.Address;
            localGroup.Postcode = lg.Postcode;
            localGroup.City = lg.City;
            localGroup.County = lg.County;
            localGroup.Description = lg.Description;
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