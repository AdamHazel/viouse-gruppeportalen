using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services;

public class PrivateUserOperations
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;

    public PrivateUserOperations(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }

    public async Task<PrivateUser> CreatePrivateUserWithPerson(PrivateUser privateUser)
    {
        if (privateUser == null) throw new ArgumentNullException(nameof(privateUser));
        
        _db.PrivateUsers.Add(privateUser);
        await _db.SaveChangesAsync();
        
        var person = new Person
        {
            Firstname = privateUser.Firstname,
            Lastname = privateUser.Lastname,
            Address = privateUser.Address,
            City = privateUser.City,
            Postcode = privateUser.Postcode,
            DateOfBirth = privateUser.DateOfBirth,
            PrivateUserId = privateUser.Id,
            PrimaryPerson = true
        };

      
        _db.Persons.Add(person);
        privateUser.Persons.Add(person);
        await _db.SaveChangesAsync();

        return privateUser;
    }
    
    
    
    public async Task AddPersonToPrivateUser(string privateUserId, Person person)
    {
        var privateUser = await _db.PrivateUsers
            .Include(p => p.Persons)
            .FirstOrDefaultAsync(p => p.Id == privateUserId);

        if (privateUser == null)
            throw new Exception("Private User not found");
        
        person.PrivateUserId = privateUserId;
        privateUser.Persons.Add(person);
        await _db.SaveChangesAsync();
    }


    public async Task<ApplicationPrivateUserViewModel> GetUserDetails(string userId)
    {
        var applicationUser = await _um.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var privateUser = await _db.PrivateUsers.FirstOrDefaultAsync(p => p.Id == userId);

        if (applicationUser == null || privateUser == null)
        {
            return null;
        }

        return new ApplicationPrivateUserViewModel
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

    public async Task<Person> getPersonDetails(string personId)
    {
        var person = await _db.Persons.FirstOrDefaultAsync(p => p.Id == personId);
        if (person == null)
        {
            return null;
        }

        return new Person
        {
            Firstname = person.Firstname,
            Lastname = person.Lastname,
            Address = person.Address,
            City = person.City,
            Postcode = person.Postcode,
            DateOfBirth = person.DateOfBirth
        };
    }
    public async Task EditPerson(Person person)
    {
        var existingPerson = await _db.Persons
            .Include(p => p.PrivateUser)
            .FirstOrDefaultAsync(p => p.Id == person.Id);

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
        
            _db.PrivateUsers.Update(privateUser);
        }

        _db.Persons.Update(existingPerson);
        await _db.SaveChangesAsync();
    }

    

    public async Task EditUserDetails(ApplicationPrivateUserViewModel viewModel)
    {
        var applicationUser = await _um.Users.FirstOrDefaultAsync(u => u.Id == viewModel.Id);
        var privateUser = await _db.PrivateUsers
            .Include(p => p.Persons) 
            .FirstOrDefaultAsync(u => u.Id == viewModel.Id);

        if (applicationUser == null || privateUser == null)
        {
            throw new Exception("ApplicationUser or PrivateUser not found with ID: " + viewModel.Id);
        }

        var person = privateUser.Persons.FirstOrDefault();
        if (person == null)
        {
            throw new Exception("No Person record found associated with this PrivateUser");
        }
        
        applicationUser.Email = viewModel.Email;
        var result = await _um.UpdateAsync(applicationUser);
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

        _db.PrivateUsers.Update(privateUser);

        var primaryPerson = privateUser.Persons.FirstOrDefault(p => p.PrimaryPerson);
        if (primaryPerson != null)
        {
            primaryPerson.Firstname = viewModel.Firstname;
            primaryPerson.Lastname = viewModel.Lastname;
            primaryPerson.Address = viewModel.Address;
            primaryPerson.City = viewModel.City;
            primaryPerson.Postcode = viewModel.Postcode;
            primaryPerson.DateOfBirth = viewModel.DateOfBirth;

            _db.Persons.Update(primaryPerson);
        }

        _db.Persons.Update(person);
        await _db.SaveChangesAsync();
    }

    public async Task DeletePerson(string personId)
    {
        var deleteperson = await _db.Persons.FirstOrDefaultAsync(p => p.Id == personId);
        if (deleteperson == null)
        {
            throw new Exception("Person not found");
        }
        if (deleteperson.PrimaryPerson)
        {
            throw new Exception($"Person {deleteperson.Firstname} {deleteperson.Lastname} cannot be deleted");
        }

        try
        {
            _db.Persons.Remove(deleteperson);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new Exception("Failed to delete Person", ex);
        }
    }
    
}
