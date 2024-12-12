using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Models.ViewModels;

namespace Gruppeportalen.Services.Interfaces;

public interface IMembershipService
{
    ResultAndMemebership? AddMembershipToDatabase(Guid membershipTypeId, string personId, Guid localGroupId);
    ResultOfOperation AddMembershipPaymentToDatabase(Guid membershipId, Guid paymentId);

    ResultOfOperation? AllowedToAddMembership(Guid membershipTypeId, string personId, Guid localGroupId);
    ResultOfOperation? RemoveMembershipById(Guid membershipId);
    Membership? GetMembershipById(Guid membershipId);
    ResultOfOperation? UpdateMembership(Membership membership);
}