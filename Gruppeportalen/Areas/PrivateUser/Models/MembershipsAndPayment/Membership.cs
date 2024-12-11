using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;

[PrimaryKey(nameof(Id))]
public class Membership
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public bool IsBlocked { get; set; }
    public bool ToBeRenewed { get; set; }
    
    [ForeignKey(nameof(MembershipTypeId))]
    public Guid? MembershipTypeId { get; set; }
    public MembershipType MembershipType { get; set; } = null!;
    
    [ForeignKey(nameof(PersonId))]
    public string? PersonId { get; set; }
    public Person Person { get; set; } = null!;
    
    [ForeignKey(nameof(LocalGroupId))]
    public Guid? LocalGroupId { get; set; }
    public LocalGroup LocalGroup { get; set; } = null!;
    
    public ICollection<MembershipPayment> MembershipPayments { get; set; } = new HashSet<MembershipPayment>();
}