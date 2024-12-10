using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Models.MembershipsAndPayment;

[PrimaryKey(nameof(MembershipId), nameof(PaymentId))]
public class MembershipPayment
{
    [ForeignKey(nameof(MembershipId))]
    public Guid MembershipId { get; set; }
    public Membership Membership { get; set; } = null!;
    
    [ForeignKey(nameof(PaymentId))]
    public Guid PaymentId { get; set; }
    public Payment Payment { get; set; } = null!;
}