using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gruppeportalen_new.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(50)]
    public string TypeOfUser { get; set; } = string.Empty;
}