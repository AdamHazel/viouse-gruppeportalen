using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen.Models;

public class ApplicationUser : IdentityUser
{
    public string TypeOfUser { get; set; }=string.Empty;
}