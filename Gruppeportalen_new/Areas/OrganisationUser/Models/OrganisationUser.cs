using System.ComponentModel.DataAnnotations.Schema;
using Gruppeportalen_new.Models;

namespace Gruppeportalen_new.Areas.OrganisationUser.Models;

public class OrganisationUser
{
   
    [ForeignKey("ApplicationUser")]
    public string Id {get; set;}
    public int OrganisationNumber { get; set; }
    
    
    public ApplicationUser? ApplicationUser { get; set; }

}