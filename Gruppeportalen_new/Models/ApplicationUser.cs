using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;


namespace Gruppeportalen_new.Models;

public class ApplicationUser : IdentityUser
{
    public string TypeOfUser { get; set; }=string.Empty;
}
