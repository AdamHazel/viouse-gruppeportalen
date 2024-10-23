using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppeportalen_new.Models;

public class OrganisationUser
{
    [ForeignKey("ApplicationUser")]
    public string Id {get; set;}
    
    [Required]
    [StringLength(100)]
    public string OrganisationNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string OrganisationName { get; set; } = string.Empty;
    
    
    public ApplicationUser? ApplicationUser { get; set; }
}