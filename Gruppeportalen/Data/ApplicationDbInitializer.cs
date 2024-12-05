using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;

using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen.Data;

public class ApplicationDbInitializer
{
    public static void Initialize(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
        
        var user = new ApplicationUser {UserName = "user@uia.no", Email = "user@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Centralorg};
        um.CreateAsync(user, "Password1.").Wait();
        
        var user1 = new ApplicationUser {UserName = "user1@uia.no", Email = "user1@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Privateuser};
        um.CreateAsync(user1, "Password1.").Wait();
        
        var user2 = new ApplicationUser {UserName = "user2@uia.no", Email = "user2@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Centralorg};
        um.CreateAsync(user2, "Password1.").Wait();
        
        var user3 = new ApplicationUser {UserName = "user3@uia.no", Email = "user3@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Privateuser};
        um.CreateAsync(user3, "Password1.").Wait();
        
        var user4 = new ApplicationUser {UserName = "user4@uia.no", Email = "user4@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Privateuser};
        um.CreateAsync(user4, "Password1.").Wait();
        
        var cUser = new CentralOrganisation {Id = user.Id, OrganisationName = "Test Org", OrganisationNumber = "12345678"};
        db.CentralOrganisations.Add(cUser);
        
        var cUser2 = new CentralOrganisation {Id = user2.Id, OrganisationName = "Test Org", OrganisationNumber = "1111111"};
        db.CentralOrganisations.Add(cUser2);
        db.SaveChanges();

        var pUser = new PrivateUser
        {
            Id = user3.Id, Firstname = "Adam", Lastname = "Hazel", DateOfBirth = new DateTime(1992, 01, 01),
            Telephone = "40567892", Address = "Random address", City = "Homborsund", Postcode = "9999",
            ApplicationUser = user3};
        db.PrivateUsers.Add(pUser);
        db.SaveChanges();
        
        var puser1 = new PrivateUser
        {
            Id = user1.Id, ApplicationUser = user1, Address = "Snarveien17 B", City = "Grimstad", Postcode = "4885", DateOfBirth = DateTime.Parse("1994-01-20"), Lastname = "Erichen", Firstname = "Nancy"
        };
        db.PrivateUsers.Add(puser1);
        db.SaveChanges();

        var pUser2 = new PrivateUser
        {
            Id = user4.Id, Firstname = "Kathe", Lastname = "Hazel", DateOfBirth = new DateTime(1993, 01, 01),
            Telephone = "91567892", Address = "Random address", City = "Homborsund", Postcode = "9999",
            ApplicationUser = user4};
        db.PrivateUsers.Add(pUser2);
        db.SaveChanges();

        var lg1 = new LocalGroup
        {
            Address = "Address 1", GroupName = "Group 1", City = "City 1", Postcode = "1111", County = "County 1",
            CentralOrganisationId = cUser.Id, Active = true,
        };
        db.LocalGroups.Add(lg1);
        
        var lg2 = new LocalGroup
        {
            Address = "Address 2", GroupName = "Group 2", City = "City 2", Postcode = "2222", County = "County 2",
            CentralOrganisationId = cUser.Id, Active = true,
        };
        db.LocalGroups.Add(lg2);
        
        var lg3 = new LocalGroup
        {
            Address = "Address 3", GroupName = "Group 3", City = "City 3", Postcode = "3333", County = "County 3",
            CentralOrganisationId = cUser.Id, Active = true,
        };
        db.LocalGroups.Add(lg3);
        
        var lg4 = new LocalGroup
        {
            Address = "Address 4", GroupName = "Group 4", City = "City 4", Postcode = "4444", County = "County 4",
            CentralOrganisationId = cUser2.Id, Active = true,
        };
        db.LocalGroups.Add(lg4);
        
        db.SaveChanges();

        var lgAdmin1 = new LocalGroupAdmin
        {
            UserId = user3.Id,
            LocalGroupId = lg1.Id,
        };
        user3.LocalGroupAdmins.Add(lgAdmin1);
        lg1.LocalGroupAdmins.Add(lgAdmin1);
        
        var lgAdmin2 = new LocalGroupAdmin
        {
            UserId = user3.Id,
            LocalGroupId = lg2.Id,
        };
        user3.LocalGroupAdmins.Add(lgAdmin2);
        lg2.LocalGroupAdmins.Add(lgAdmin2);
        
        var lgAdmin3 = new LocalGroupAdmin
        {
            UserId = user3.Id,
            LocalGroupId = lg3.Id,
        };
        user3.LocalGroupAdmins.Add(lgAdmin3);
        lg3.LocalGroupAdmins.Add(lgAdmin3);
        
        var lgAdmin4 = new LocalGroupAdmin
        {
            UserId = user3.Id,
            LocalGroupId = lg4.Id,
        };
        user3.LocalGroupAdmins.Add(lgAdmin4);
        lg4.LocalGroupAdmins.Add(lgAdmin4);
        
        var lgAdmin5 = new LocalGroupAdmin
        {
            UserId = user4.Id,
            LocalGroupId = lg3.Id,
        };
        user4.LocalGroupAdmins.Add(lgAdmin5);
        lg3.LocalGroupAdmins.Add(lgAdmin5);
        
        db.SaveChanges();
    }
}
