﻿using Gruppeportalen.Areas.CentralOrganisation.Models;
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
        
        var puser1 = new PrivateUser
        {
            Id = user1.Id, ApplicationUser = user1, Address = "Snarveien17 B", City = "Grimstad", Postcode = "4885", DateOfBirth = DateTime.Parse("1994-01-20"), Lastname = "Erichen", Firstname = "Nancy"
        };
        db.PrivateUsers.Add(puser1);
        
        var cUser = new CentralOrganisation {Id = user.Id, OrganisationName = "Test Org", OrganisationNumber = "12345678"};
        db.CentralOrganisations.Add(cUser);
        
        var cUser2 = new CentralOrganisation {Id = user2.Id, OrganisationName = "Test Org", OrganisationNumber = "1111111"};
        db.CentralOrganisations.Add(cUser2);
        db.SaveChanges();
        
        db.LocalGroups.Add(new LocalGroup
        {
            Address = "Address 1", GroupName = "Group 1", City = "City 1", Postcode = "1111", County = "Oslo",
            CentralOrganisationId = cUser.Id, Active = true
        });
        
        db.LocalGroups.Add(new LocalGroup
        {
            Address = "Address 2", GroupName = "Group 2", City = "City 2", Postcode = "2222", County = "Oslo",
            CentralOrganisationId = cUser.Id, Active = true
        });
        
        db.LocalGroups.Add(new LocalGroup
        {
            Address = "Address 3", GroupName = "Group 3", City = "City 3", Postcode = "3333", County = "Møre og Romsdalen",
            CentralOrganisationId = cUser.Id, Active = true
        });
        
        db.LocalGroups.Add(new LocalGroup
        {
            Address = "Address 4", GroupName = "Group 4", City = "City 4", Postcode = "4444", County = "Agder",
            CentralOrganisationId = cUser2.Id, Active = false
        });
        
        db.SaveChanges();
    }
}
