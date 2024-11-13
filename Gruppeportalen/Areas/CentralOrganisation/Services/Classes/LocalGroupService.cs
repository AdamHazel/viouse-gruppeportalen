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
    

    public bool AddNewLocalGroup(LocalGroup localGroup, string organisationId)
    {
        localGroup.CentralOrganisationId = organisationId;
        return _addGroupToDb(localGroup);
    }

    public List<LocalGroup> GetLocalGroups(string organisationId)
    {
        var groups = _db.LocalGroups.Where(g => g.CentralOrganisationId == organisationId).ToList();
        return groups;
    }
   
    public List<string> GetAllCounties()
    {
        return new List<string>(Constants.Counties); 
    }
}