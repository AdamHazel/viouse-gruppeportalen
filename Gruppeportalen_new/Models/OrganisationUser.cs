using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppeportalen_new.Models;

public class OrganisationUser
{
    [Key]
    [ForeignKey("ApplicationUser")]
    public string UserId {get; set;}
    
    [Required]
    [StringLength(100)]
    public string OrganisationNumber { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string OrganisationName { get; set; } = string.Empty;
    
    
    public ApplicationUser? ApplicationUser { get; set; }
}