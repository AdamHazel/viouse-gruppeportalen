using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen.Data;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        var user = new ApplicationUser {UserName = "user@uia.no", Email = "user@uia.no", EmailConfirmed = true, TypeOfUser = "PrivateUser"};
        um.CreateAsync(user, "Password1.").Wait();
        
        db.SaveChanges();
    }
}