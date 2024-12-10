using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Models;
using Gruppeportalen.Models.MembershipsAndPayment;

namespace Gruppeportalen.Models;

public class PrivateUser
{
    [Key]
    [ForeignKey(nameof(ApplicationUser))]
    public string Id {get; set;}
    
    [Required]
    [StringLength(30)]
    public string Firstname { get; set; } = string.Empty;

    [Required] 
    [StringLength(30)] 
    public string Lastname { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Address { get; set; } = String.Empty;

    [Required]
    [StringLength(30)]
    public string City { get; set; } = String.Empty;

    [Required]
    [MinLength(4)]
    [StringLength(4)]
    [PostcodeFormatNumbersValidation]
    public string Postcode { get; set; } = String.Empty;

    [Required]
    [StringLength(30)]
    public string Telephone { get; set; } = String.Empty;

    
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    
    public ApplicationUser? ApplicationUser{ get; set; }

    public ICollection<UserPersonConnection> UserPersonConnections { get; set; } = new HashSet<UserPersonConnection>();
    public ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public ICollection<Person> Persons { get; set; } = new List<Person>();
    
}