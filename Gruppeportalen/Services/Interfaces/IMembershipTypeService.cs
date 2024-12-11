using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IMembershipTypeService
{
    bool AddNewMembershipType(MembershipType membershipType, Guid groupId);
    
    MembershipType? GetMembershipTypeById(Guid id);

    bool UpdateMembershipType(MembershipType membershipType);
    
}