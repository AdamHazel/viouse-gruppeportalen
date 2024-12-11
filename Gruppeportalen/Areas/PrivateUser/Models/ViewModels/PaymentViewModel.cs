namespace Gruppeportalen.Areas.PrivateUser.Models.ViewModels;

public class PaymentViewModel
{
    public Guid MembershipTypeId { get; set; }
    public string MembershipName { get; set; } = string.Empty;
    public int Price { get; set; }
    public string Nonce { get; set; } = string.Empty;
    public string? ErrorMessage { get; set; }
    
}