using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Gruppeportalen.Services.Classes;

public class PrivateUserOperations : IPrivateUserOperations
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;

    public PrivateUserOperations(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }

    private bool _addPrivateUserToDb(PrivateUser privateUser)
    {
        try
        {
            _db.PrivateUsers.Add(privateUser);
            if (_db.SaveChanges() > 0)
                return true;
            else
                throw new DbUpdateException("Failed to add group to db");
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }
    public bool AddPrivateUserToDb(PrivateUser privateUser)
    {
        return _addPrivateUserToDb(privateUser);
    }
    
    public ApplicationPrivateUser? GetUserDetails(string userId)
    {
        try
        {
            var applicationUser = _um.Users.FirstOrDefault(u => u.Id == userId);
            var privateUser = _db.PrivateUsers.FirstOrDefault(p => p.Id == userId);

            if (applicationUser == null || privateUser == null)
            {
                return null; 
            }

            return new ApplicationPrivateUser
            {
                Id = applicationUser.Id,
                Email = applicationUser.Email,
                Telephone = privateUser.Telephone,
                Firstname = privateUser.Firstname,
                Lastname = privateUser.Lastname,
                Address = privateUser.Address,
                City = privateUser.City,
                Postcode = privateUser.Postcode,
                DateOfBirth = privateUser.DateOfBirth
            };
        }
        catch (Exception ex)
        {
            return null;
        }
    }


    public Person? GetPersonDetails(string personId)
    {
        try
        {
            var person = _db.Persons.FirstOrDefault(p => p.Id == personId);
            
            if (person == null)
            {
                return null;
            }
            return person;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public void EditPerson(Person person)
    {
        try
        {
            var existingPerson = _db.Persons
                .Include(p => p.PrivateUser)
                .FirstOrDefault(p => p.Id == person.Id);
            
            if (existingPerson == null)
            {
                throw new Exception("Person not found");
            }
            
            existingPerson.Firstname = person.Firstname;
            existingPerson.Lastname = person.Lastname;
            existingPerson.Address = person.Address;
            existingPerson.City = person.City;
            existingPerson.Postcode = person.Postcode;
            existingPerson.DateOfBirth = person.DateOfBirth;
            
            if (existingPerson.PrimaryPerson && existingPerson.PrivateUser != null)
            {
                var privateUser = existingPerson.PrivateUser;
                privateUser.Firstname = person.Firstname;
                privateUser.Lastname = person.Lastname;
                privateUser.Address = person.Address;
                privateUser.City = person.City;
                privateUser.Postcode = person.Postcode;
                privateUser.DateOfBirth = person.DateOfBirth;
            }
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"En databasefeil oppstod: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En uventet feil oppstod: {ex.Message}");
        }
    }
    
    

   public void EditUserDetails(ApplicationPrivateUser viewModel)
    {
        try
        {
            var applicationUser = _um.Users.FirstOrDefault(u => u.Id == viewModel.Id);
            var privateUser = _db.PrivateUsers
                .Include(p => p.Persons)
                .FirstOrDefault(u => u.Id == viewModel.Id);

            if (applicationUser == null || privateUser == null)
            {
                throw new Exception($"ApplicationUser or PrivateUser not found with ID: {viewModel.Id}");
            }
            var person = privateUser.Persons.FirstOrDefault();
            if (person == null)
            {
                throw new Exception("No Person record found associated with this PrivateUser");
            }
            applicationUser.Email = viewModel.Email;
            var result = _um.UpdateAsync(applicationUser).Result;
            if (!result.Succeeded)
            {
                throw new Exception("Failed to update ApplicationUser");
            }
            privateUser.Telephone = viewModel.Telephone;
            privateUser.Firstname = viewModel.Firstname;
            privateUser.Lastname = viewModel.Lastname;
            privateUser.Address = viewModel.Address;
            privateUser.City = viewModel.City;
            privateUser.Postcode = viewModel.Postcode;
            privateUser.DateOfBirth = viewModel.DateOfBirth;

            var primaryPerson = privateUser.Persons.FirstOrDefault(p => p.PrimaryPerson);
            if (primaryPerson != null)
            {
                primaryPerson.Firstname = viewModel.Firstname;
                primaryPerson.Lastname = viewModel.Lastname;
                primaryPerson.Address = viewModel.Address;
                primaryPerson.City = viewModel.City;
                primaryPerson.Postcode = viewModel.Postcode;
                primaryPerson.DateOfBirth = viewModel.DateOfBirth;
            }
            
            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"En feil oppstod: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En uventet feil oppstod: {ex.Message}");
        }
    }
   
    public IEnumerable<LocalGroup> GetAllLocalActiveGroups()
    {
        var groups = _db.LocalGroups
            .Include(g => g.MembershipTypes)
            .OrderBy(g => g.GroupName)
            .Where(g => g.Active).ToList();

        if (!groups.IsNullOrEmpty())
        {
            foreach (var group in groups)
            {
                group.MembershipTypes = group.MembershipTypes
                    .OrderBy(m => m.MembershipName).ToList();
            }
        }
        return groups;
    }

    public List<string> GetAllCounties()
    {
        return new List<string>(Constants.Counties); 
    }
        
    public IEnumerable<LocalGroup> SearchLocalGroups(string query, string county)
    {
        var localGroups = _db.LocalGroups
            .Include(g => g.MembershipTypes)
            .OrderBy(g => g.GroupName)
            .Where(g => g.Active).AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            localGroups = localGroups.Where(g => g.GroupName.ToLower().Contains(query.ToLower()));
        }

        if (!string.IsNullOrEmpty(county))
        {
            localGroups = localGroups.Where(g => g.County.ToLower() == county.ToLower());
        }

        var result = localGroups.ToList();
        return result;
    }

    public bool PrivateUserExists(string id)
    {
        return _db.PrivateUsers.Any(u => u.Id == id);
    }

    public PrivateUser? GetPrivateUserById(string id)
    {
        var user = _db.PrivateUsers
            .Include(pu => pu.Payments)
            .ThenInclude(mp => mp.MembershipPayments)
            .Include(upc => upc.UserPersonConnections)
            .ThenInclude(pe => pe.Person)
            .FirstOrDefault(u => u.Id == id);

        if (user != null)
        {
            user.UserPersonConnections = user.UserPersonConnections
                .OrderBy(upc => upc.Person.Lastname)
                .ThenBy(upc => upc.Person.Firstname)
                .ToList();
        }

        return user;
    }

    public PrivateUser? GetPrivateUserByIdWithConnectedPersons(string id)
    {
        var privateUser = _db.PrivateUsers
            .Include(upc => upc.UserPersonConnections)
                .ThenInclude(up => up.Person)
                .ThenInclude(p => p.Memberships)
                .ThenInclude(m => m.MembershipType)
            .Include(upc => upc.UserPersonConnections)
                .ThenInclude(up => up.Person)
                .ThenInclude(p => p.Memberships)
                .ThenInclude(m => m.LocalGroup)
            .FirstOrDefault(u => u.Id == id);

        if (privateUser != null)
        {
            privateUser.UserPersonConnections = privateUser.UserPersonConnections
                .OrderBy(upc => upc.Person.Lastname ?? string.Empty)
                .ThenBy(upc => upc.Person.Firstname ?? string.Empty)
                .ToList();
        }
        
        return privateUser;
    }
    
}
