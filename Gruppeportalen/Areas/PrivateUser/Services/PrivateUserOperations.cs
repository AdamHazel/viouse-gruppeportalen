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

    public PrivateUser CreatePrivateUserWithPerson(PrivateUser privateUser)
    {
        if (privateUser == null) throw new ArgumentNullException(nameof(privateUser));

        try
        {
            _db.PrivateUsers.Add(privateUser);
            _db.SaveChanges(); 
            
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
            _db.SaveChanges(); 

            return privateUser; 
        }
        catch (DbUpdateException ex)
        {
            return null; 
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    
    
    
    public void AddPersonToPrivateUser(string privateUserId, Person person)
    {
        try
        {
            var privateUser = _db.PrivateUsers
                .Include(p => p.Persons)
                .FirstOrDefault(p => p.Id == privateUserId);

            if (privateUser == null)
                throw new Exception("Private User not found");

            person.PrivateUserId = privateUserId;
            privateUser.Persons.Add(person);

            _db.SaveChanges();
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"En feil oppstod: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod: {ex.Message}");
        }
    }
    

    public ApplicationPrivateUser GetUserDetails(string userId)
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


    public Person GetPersonDetails(string personId)
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

public void DeletePerson(string personId)
{
    try
    {
        var deletePerson = _db.Persons.FirstOrDefault(p => p.Id == personId);
        if (deletePerson == null)
        {
            throw new Exception("Person not found");
        }
        if (deletePerson.PrimaryPerson)
        {
            throw new Exception($"Person {deletePerson.Firstname} {deletePerson.Lastname} cannot be deleted because they are the primary person.");
        }
        _db.Persons.Remove(deletePerson);
        _db.SaveChanges(); 
    }
    catch (DbUpdateException ex)
    {
        throw new Exception("En uventet feil oppstod:", ex);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"En uventet feil oppstod: {ex.Message}");
        throw; 
    }
}

    
}
