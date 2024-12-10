using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Models.MembershipsAndPayment;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class MembershipTypeService : IMembershipTypeService
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<LocalGroupService> _logger;
   
    
    public MembershipTypeService(ApplicationDbContext db, ILogger<LocalGroupService> logger)
    {
        _db = db;
        _logger = logger;
    }
    private bool _addMembershipTypeToDb(MembershipType mt, LocalGroup lg)
    {
        try
        {
            _db.MembershipTypes.Add(mt);
            lg.MembershipTypes.Add(mt);
            return _db.SaveChanges() > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError($"Failed to add membership type: {ex.Message}");
            return false;
        }
    }

    public bool AddNewMembershipType(MembershipType membershipType, Guid groupId)
    {
        try
        {
            var localGroup = _db.LocalGroups
                .Include(g => g.MembershipTypes)
                .FirstOrDefault(g => g.Id == groupId);
            if (localGroup == null)
            {
                return false;
            }
            
            var existingMembership = _db.MembershipTypes
                .FirstOrDefault(m => m.MembershipName == membershipType.MembershipName && m.LocalGroupId == groupId);
            if (existingMembership != null)
            {
                return false; 
            }
            
            membershipType.LocalGroupId = groupId;
            return _addMembershipTypeToDb(membershipType, localGroup);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public MembershipType? GetMembershipTypeById(Guid id)
    {
        return _db.MembershipTypes.FirstOrDefault(mt => mt.Id == id);
    }

    public bool UpdateMembershipType(MembershipType membershipType)
    {
        try
        {
            _db.MembershipTypes.Update(membershipType);
            return _db.SaveChanges() > 0;
        }
        catch (DbUpdateException ex)
        {
            return false;
        }
    }
}