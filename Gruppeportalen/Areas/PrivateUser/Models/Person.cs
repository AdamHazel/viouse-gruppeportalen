using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen.Areas.CentralOrganisation.DataAnnotations;
using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.PrivateUser.Models;

public class Person
{
    [Key]
    public string Id {get; set;} = Guid.NewGuid().ToString();
    
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
    [MaxLength(4)]
    [PostcodeFormatNumbersValidation]
    public string Postcode { get; set; } = String.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    
    public bool PrimaryPerson { get; set; } = false;
    
    public ICollection<UserPersonConnection> UserPersonConnections { get; set; } = new HashSet<UserPersonConnection>();
    
    [ForeignKey(nameof(PrivateUser))]
    public string? PrivateUserId { get; set; }
    public Gruppeportalen.Models.PrivateUser? PrivateUser { get; set; }
    
    
    
}