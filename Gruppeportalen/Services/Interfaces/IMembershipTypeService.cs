using Gruppeportalen.Models;
using Gruppeportalen.Models.MembershipsAndPayment;

namespace Gruppeportalen.Services.Interfaces;

public interface IMembershipTypeService
{
    bool AddNewMembershipType(MembershipType membershipType, Guid groupId);
    
    MembershipType? GetMembershipTypeById(Guid id);

    bool UpdateMembershipType(MembershipType membershipType);
    
}