using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gruppeportalen.Areas.PrivateUser.Models;

public class Person
{
    [Key]
    public Guid Id {get; set;} =Guid.NewGuid();
    
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
    [StringLength(4)]
    public string Postcode { get; set; } = String.Empty;

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    
    public bool PrimaryPerson { get; set; } = false;
    
    [ForeignKey(nameof(PrivateUser))]
    public string? PrivateUserId { get; set; }
    public Gruppeportalen.Models.PrivateUser? PrivateUser { get; set; }
    
    public ICollection<SharedPerson> SharedPersons { get; set; } = new List<SharedPerson>();
    
}