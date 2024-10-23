using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen_new.Data;

public class ApplicationDbInitialiser
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

       var user = new ApplicationUser {UserName = "user@uia.no", Email = "user@uia.no", EmailConfirmed = true, UserType =UserType.PrivateUser};
        um.CreateAsync(user, "Password1.").Wait();
        
        var user1 = new ApplicationUser {UserName = "user1@uia.no", Email = "user1@uia.no", EmailConfirmed = true, UserType = UserType.OrganisationUser};
        um.CreateAsync(user1, "Password1.").Wait();
        
       // var privateuser = new NormalUser{Firstname = "FirstName", Lastname = "LastName", Id = user.Id, ApplicationUser = user, Country = "Country", DateOfBirth = new DateTime(1990, 01, 01), };
        //var organisationuser = new OrganisationUser{OrganisationNumber = 123, Id = user1.Id, ApplicationUser = user1};
        
        //db.NormalUsers.Add(privateuser);
        //db.OrganisationUsers.Add(organisationuser);

        db.SaveChanges();
    }
}