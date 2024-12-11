using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;

namespace Gruppeportalen.Services.Interfaces;

public interface IPaymentService
{
    ResultOfOperation? AddPayment(Payment p);
    ResultOfOperation? AddMemberPayment(Guid paymentId, Guid membershipId);
    Payment? GetPaymentById(Guid paymentId);
    ResultOfOperation? RemovePaymentById(Guid paymentId);

}