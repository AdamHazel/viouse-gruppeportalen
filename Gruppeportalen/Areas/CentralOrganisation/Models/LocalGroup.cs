using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Gruppeportalen.Areas.CentralOrganisation.Models;

public class LocalGroup
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [Required]
    [StringLength(50)]
    [DisplayName("Navn av lokallag")]
    public string GroupName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(30)]
    [DisplayName("Adresse")]
    public string Address { get; set; } = string.Empty;
    
    [Required]
    [StringLength(30)]
    [DisplayName("By")]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    [DisplayName("Sted")]
    public string County { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    [MaxLength(4)]
    [PostcodeFormatNumbersValidation]
    [DisplayName("Postnummer")]
    public string Postcode { get; set; } = string.Empty;
    
    // Foreign key
    [ForeignKey("CentralOrganisation")]
    public string? CentralOrganisationId { get; set; }
    public CentralOrganisation? Organisation { get; set; }
    
    // Values that are possible to be null
    [StringLength(50)]
    public string? Description { get; set; }
    
    // Values for running of local group
    public bool Active { get; set; } = false;

}