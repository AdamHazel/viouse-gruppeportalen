namespace Gruppeportalen.Areas.ViewModels;

public class ApplicationPrivateUserViewModel
{
    // Felter fra ApplicationUser
    public string Id { get; set; }
    public string Email { get; set; }
    
    // Felter fra PrivateUser
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Postcode { get; set; }
    
    public string Telephone { get; set; }
    public DateTime DateOfBirth { get; set; }
}