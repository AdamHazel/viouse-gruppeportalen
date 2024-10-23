using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

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