using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;

[PrimaryKey(nameof(Id))]
public class Payment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Amount { get; set; }
    public bool Paid { get; set; } = false;
    public DateTime? PaymentDate { get; set; }
    
    [ForeignKey(nameof(PaidByUserId))]
    public string? PaidByUserId { get; set; }
    public Gruppeportalen.Models.PrivateUser? PaidByUser { get; set; }
    
    public ICollection<MembershipPayment> MembershipPayments { get; set; } = new HashSet<MembershipPayment>();
}