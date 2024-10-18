using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen_new.Data;

public class ApplicationDbInitialiser
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        var user = new ApplicationUser {UserName = "user@uia.no", Email = "user@uia.no", EmailConfirmed = true };
        um.CreateAsync(user, "Password1.").Wait();

        db.SaveChanges();
    }
}