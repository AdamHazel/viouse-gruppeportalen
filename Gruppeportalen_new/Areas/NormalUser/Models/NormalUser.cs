using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen_new.Models;

namespace Gruppeportalen_new.Areas.NormalUser.Models;

public class NormalUser
{
    [ForeignKey("ApplicationUser")]
    public string Id {get; set;}
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    
    public string Country { get; set; }
    
    [DataType(DataType.DateTime)]
    public DateTime DateOfBirth { get; set; }
    
    
    public ApplicationUser? ApplicationUser{ get; set; }
}