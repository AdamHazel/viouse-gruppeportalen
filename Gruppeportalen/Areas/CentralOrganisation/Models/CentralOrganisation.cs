using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.CentralOrganisation.Models;

public class CentralOrganisation
{
    [Key]
    [ForeignKey("ApplicationUser")]
    public string Id {get; set;}
    
    [Required]
    [MaxLength(100)]
    public string OrganisationNumber { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string OrganisationName { get; set; } = string.Empty;
    
    
    public ApplicationUser? ApplicationUser { get; set; }
}