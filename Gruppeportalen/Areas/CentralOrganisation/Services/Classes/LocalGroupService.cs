using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Data;
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
    private readonly List<string> _counties = new List<string> { "Akershus", "Oslo", "Vestland", "Trøndelag", "Innlandet", "Agder", "Østfold", "Møre og Romsdalen", "Buskerud", "Vestfold", "Nordland", "Telemark", "Troms", "Finnmark"};
    
    
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
    public IEnumerable<LocalGroup> GetAllLocalGroups()
    {
        return _db.LocalGroups.ToList();
    }


    public List<string> GetAllCounties()
    {
        return new List<string>(_counties); 
    }
    
    public IEnumerable<LocalGroup> SearchLocalGroups(string query, string county)
    {
        var localGroups = _db.LocalGroups.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            localGroups = localGroups.Where(g => g.GroupName.ToLower().Contains(query.ToLower()));
        }

        if (!string.IsNullOrEmpty(county))
        {
            localGroups = localGroups.Where(g => g.County.ToLower() == county.ToLower());
        }

        var result = localGroups.ToList();
        return result;
    }
}