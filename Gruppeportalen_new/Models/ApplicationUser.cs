using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;


namespace Gruppeportalen_new.Models;

public class ApplicationUser : IdentityUser
{
    public UserType UserType{ get; set; }
}
public enum UserType
{
    OrganisationUser,
    PrivateUser
}