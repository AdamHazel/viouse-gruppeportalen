namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class PaymentListViewModel
{
    public Guid PaymentId { get; set; }
    public Guid MembershipId { get; set; }
    public string MemberName { get; set; } = String.Empty;
    public string MembershipName { get; set; } = string.Empty;
    public int Amount { get; set; }
    public bool Paid { get; set; } 
    public DateTime? PaymentDate { get; set; }
    public string ValidityPeriod { get; set; } = string.Empty;
}