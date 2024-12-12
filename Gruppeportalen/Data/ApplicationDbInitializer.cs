using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
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
        
        var user = new ApplicationUser {UserName = "football@lag.no", Email = "football@lag.no", EmailConfirmed = true, TypeOfUser = Constants.Centralorg};
        um.CreateAsync(user, "Password1.").Wait();
        
        var user1 = new ApplicationUser {UserName = "hans@uia.no", Email = "hans@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Privateuser};
        um.CreateAsync(user1, "Password1.").Wait();
        
        var user2 = new ApplicationUser {UserName = "bjj@sport.no", Email = "bjj@sport.no", EmailConfirmed = true, TypeOfUser = Constants.Centralorg};
        um.CreateAsync(user2, "Password1.").Wait();
        
        var user3 = new ApplicationUser {UserName = "lisa@uia.no", Email = "lisa@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Privateuser};
        um.CreateAsync(user3, "Password1.").Wait();
        
        var user4 = new ApplicationUser {UserName = "kari@uia.no", Email = "kari@uia.no", EmailConfirmed = true, TypeOfUser = Constants.Privateuser};
        um.CreateAsync(user4, "Password1.").Wait();
        
        var cUser = new CentralOrganisation {Id = user.Id, OrganisationName = "Grimstad Fotballklubb", OrganisationNumber = "12345678"};
        db.CentralOrganisations.Add(cUser);
        
        var cUser2 = new CentralOrganisation {Id = user2.Id, OrganisationName = "Sirdal Håndballklubb", OrganisationNumber = "1111111"};
        db.CentralOrganisations.Add(cUser2);
        db.SaveChanges();

        var Adam = new PrivateUser
        {
            Id = user3.Id, Firstname = "Lisa", Lastname = "Larsen", DateOfBirth = new DateTime(1992, 01, 01),
            Telephone = "40567892", Address = "Holvikaholsen 45", City = "Grimstad", Postcode = "4879",
            ApplicationUser = user3};
        db.PrivateUsers.Add(Adam);
        db.SaveChanges();
        
        var person = new Person
        {
            Id = Adam.Id,
            Firstname = Adam.Firstname,
            Lastname = Adam.Lastname,
            Address = Adam.Address,
            City = Adam.City,
            Postcode = Adam.Postcode,
            DateOfBirth = Adam.DateOfBirth,
            PrivateUserId = Adam.Id,
            PrimaryPerson = true
        };
        db.Persons.Add(person);
        db.SaveChanges();
        
        var upc1 = new UserPersonConnection { PrivateUserId = Adam.Id, PersonId = person.Id };
        Adam.UserPersonConnections.Add(upc1);
        person.UserPersonConnections.Add(upc1);
        db.SaveChanges();
        
        var Blah = new PrivateUser
        {
            Id = user1.Id, 
            ApplicationUser = user1, 
            Address = "Snarveien 17B", 
            City = "Grimstad", 
            Postcode = "4885", 
            DateOfBirth = DateTime.Parse("1994-01-20"), 
            Lastname = "Hansen", 
            Firstname = "Hans"
        };
        db.PrivateUsers.Add(Blah);
        db.SaveChanges();
        
        var person1 = new Person
        {
            Id = Blah.Id,
            Firstname = Blah.Firstname,
            Lastname = Blah.Lastname,
            Address = Blah.Address,
            City = Blah.City,
            Postcode = Blah.Postcode,
            DateOfBirth = Blah.DateOfBirth,
            PrivateUserId = Blah.Id,
            PrimaryPerson = true
        };
        db.Persons.Add(person1);
        db.SaveChanges();
        
        var upc2 = new UserPersonConnection { PrivateUserId = Blah.Id, PersonId = person1.Id };
        Blah.UserPersonConnections.Add(upc2);
        person1.UserPersonConnections.Add(upc2);
        db.SaveChanges();
        
        var Kathe = new PrivateUser
        {
            Id = user4.Id, 
            Firstname = "Kari", 
            Lastname = "Nordmann", 
            DateOfBirth = new DateTime(1993, 01, 01),
            Telephone = "91567892", 
            Address = "Solveien 30", 
            City = "Sandefjord", 
            Postcode = "3217",
            ApplicationUser = user4,
        };
        db.PrivateUsers.Add(Kathe);
        db.SaveChanges();
        
        var person2 = new Person
        {
            Id = Kathe.Id,
            Firstname = Kathe.Firstname,
            Lastname = Kathe.Lastname,
            Address = Kathe.Address,
            City = Kathe.City,
            Postcode = Kathe.Postcode,
            DateOfBirth = Kathe.DateOfBirth,
            PrivateUserId = Kathe.Id,
            PrimaryPerson = true
        };
        db.Persons.Add(person2);
        db.SaveChanges();
        
        var upc3 = new UserPersonConnection { PrivateUserId = Kathe.Id, PersonId = person2.Id };
        Kathe.UserPersonConnections.Add(upc3);
        person2.UserPersonConnections.Add(upc3);
        db.SaveChanges();
        
        var person3 = new Person
        {
            Firstname = "Nansi",
            Lastname = "Nordmann",
            Address = "Dagveien 76",
            City = "Homborsund",
            Postcode = "4888",
            DateOfBirth = new DateTime(2003, 01, 01),
        };
        db.Persons.Add(person3);
        db.SaveChanges();
        
        var person4 = new Person
        {
            Firstname = "Tirill",
            Lastname = "Eckhoff",
            Address = "Fjellveien 26",
            City = "Oslo",
            Postcode = "0001",
            DateOfBirth = new DateTime(2003, 01, 01),
        };
        db.Persons.Add(person4);
        db.SaveChanges();
        
        var person5 = new Person
        {
            Firstname = "Tarjei",
            Lastname = "Bø",
            Address = "Stryneveien 205",
            City = "Stryn",
            Postcode = "6259",
            DateOfBirth = new DateTime(1988, 05, 3),
        };
        db.Persons.Add(person5);
        db.SaveChanges();
        
        
        var upc4 = new UserPersonConnection { PrivateUserId = Blah.Id, PersonId = person3.Id };
        Blah.UserPersonConnections.Add(upc4);
        person3.UserPersonConnections.Add(upc4);
        db.SaveChanges();
        
        var upc5 = new UserPersonConnection { PrivateUserId = Blah.Id, PersonId = person4.Id };
        Blah.UserPersonConnections.Add(upc5);
        person4.UserPersonConnections.Add(upc5);
        db.SaveChanges();
        
        var upc7 = new UserPersonConnection { PrivateUserId = Kathe.Id, PersonId = person5.Id };
        Kathe.UserPersonConnections.Add(upc7);
        person5.UserPersonConnections.Add(upc7);
        db.SaveChanges();
        
        var upc8 = new UserPersonConnection { PrivateUserId = Adam.Id, PersonId = person1.Id };
        Adam.UserPersonConnections.Add(upc8);
        person1.UserPersonConnections.Add(upc8);
        db.SaveChanges();
        

        var lg1 = new LocalGroup
        {
            Address = "Svingen 1", 
            GroupName = "Grimstad 15+", 
            City = "Grimstad", 
            Postcode = "4887", 
            County = "Agder",
            CentralOrganisationId = cUser.Id, 
            Active = true, 
            Description = "Ukentlig fotballtrening for alle som er over 15.",
        };
        db.LocalGroups.Add(lg1);
        
        var lg2 = new LocalGroup
        {
            Address = lg1.Address, 
            GroupName = "Grimstad 6+", 
            City = lg1.City, 
            Postcode = lg1.Postcode, 
            County = lg1.County,
            CentralOrganisationId = cUser.Id, 
            Active = true, 
            Description = "Ukentlig fotballtrening for alle mellom 6 og 15."
        };
        db.LocalGroups.Add(lg2);
        
        var lg3 = new LocalGroup
        {
            Address = "Bølgeveien 5", 
            GroupName = "Turneringsklubben", 
            City = "Arendal", 
            Postcode = "3111", 
            County = "Agder",
            CentralOrganisationId = cUser2.Id, 
            Active = true, 
            Description = "En gruppe for alle som ønsker å konkurrere i BJJ turneringer."
        };
        db.LocalGroups.Add(lg3);
        
        var lg4 = new LocalGroup
        {
            Address = lg3.Address, 
            GroupName = "Pensjonistklubben", 
            City = lg3.City, 
            Postcode = lg3.Postcode, 
            County = lg3.County,
            CentralOrganisationId = cUser2.Id, 
            Active = true, 
            Description = "Morsom trening for alle som er over 65."
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

        var mtp1 = new MembershipType
        {
            MembershipName = "Enkeltmedlemskap",
            DayReset = 15,
            MonthReset = 9,
            Price = 150,
            LocalGroupId = lg1.Id,
        };
        db.MembershipTypes.Add(mtp1);
        
        var mtp2 = new MembershipType
        {
            MembershipName = "Premiummedlemskap",
            DayReset = 27,
            MonthReset = 9,
            Price = 100,
            LocalGroupId = lg1.Id,
        };
        db.MembershipTypes.Add(mtp2);
        
        var mtp3 = new MembershipType
        {
            MembershipName = "Studentmedlemskap",
            DayReset = 11,
            MonthReset = 12,
            Price = 100,
            LocalGroupId = lg2.Id,
        };
        db.MembershipTypes.Add(mtp3);
        
        var mtp4 = new MembershipType
        {
            MembershipName = "Enkeltmedlemskap",
            DayReset = 15,
            MonthReset = 12,
            Price = 1000,
            LocalGroupId = lg2.Id,
        };
        db.MembershipTypes.Add(mtp4);
        
        var mtp5 = new MembershipType
        {
            MembershipName = "Enkeltmedlemskap",
            DayReset = 15,
            MonthReset = 12,
            Price = 1000,
            LocalGroupId = lg3.Id,
        };
        db.MembershipTypes.Add(mtp5);
        
        var mtp6 = new MembershipType
        {
            MembershipName = "Supermedlemskap",
            DayReset = 15,
            MonthReset = 12,
            Price = 1000,
            LocalGroupId = lg3.Id,
        };
        db.MembershipTypes.Add(mtp6);
        
        db.SaveChanges();
    }
}
