using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Gruppeportalen.Models;

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
    [DisplayName("Sted")]
    public string City { get; set; } = string.Empty;
    
    [Required]
    [StringLength(20)]
    [DisplayName("Fylke")]
    public string County { get; set; } = string.Empty;

    [Required]
    [MinLength(4)]
    [MaxLength(4)]
    [PostcodeFormatNumbersValidation]
    [DisplayName("Postnummer")]
    public string Postcode { get; set; } = string.Empty;
    
    // Foreign key
    [ForeignKey(nameof(Organisation))]
    public string? CentralOrganisationId { get; set; }
    public CentralOrganisation? Organisation { get; set; }
    
    // Values that are possible to be null
    [StringLength(50)]
    public string? Description { get; set; }
    
    // Values for running of local group
    public bool Active { get; set; } = false;
    
    public ICollection<LocalGroupAdmin> LocalGroupAdmins { get; set; } = new List<LocalGroupAdmin>();
    public ICollection<MembershipType> MembershipTypes { get; set; } = new List<MembershipType>();
    
    public ICollection<Membership> Memberships { get; set; } = new HashSet<Membership>();
}