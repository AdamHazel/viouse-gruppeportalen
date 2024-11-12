using Gruppeportalen.Areas.CentralOrganisation.Models;
using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen.Models;

public class ApplicationUser : IdentityUser
{
    public string TypeOfUser { get; set; }=string.Empty;
    
    // Admins connected to Private User
    public ICollection<LocalGroupAdmin> LocalGroupAdmins { get; set; } = new List<LocalGroupAdmin>();
}