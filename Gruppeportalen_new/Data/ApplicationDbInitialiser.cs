using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen_new.Data;

public class ApplicationDbInitialiser
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um, RoleManager<IdentityRole> rm)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        // Create roles
        var normalUser = new IdentityRole("NormalUser");
        rm.CreateAsync(normalUser).Wait();
        var centralOrg = new IdentityRole("centralOrg");
        rm.CreateAsync(centralOrg).Wait();
        
        // Add users
        var user = new ApplicationUser {UserName = "user@uia.no", Email = "user@uia.no", EmailConfirmed = true };
        um.CreateAsync(user, "Password1.").Wait();
        
        var user2 = new ApplicationUser {UserName = "user2@uia.no", Email = "user2@uia.no", EmailConfirmed = true };
        um.CreateAsync(user2, "Password1.").Wait();
        
        // Add user to role
        um.AddToRoleAsync(user,"NormalUser").Wait();
        
        db.SaveChanges();
    }
}