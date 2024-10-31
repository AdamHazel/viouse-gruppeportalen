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
        // Sjekker at nødvendige felter i PrivateUser er satt
        if (privateUser == null) throw new ArgumentNullException(nameof(privateUser));

        // Oppretter og lagrer PrivateUser først
        _db.PrivateUsers.Add(privateUser);
        await _db.SaveChangesAsync();

        // Opprett Person tilknyttet til den lagrede PrivateUser
        var person = new Person
        {
            Firstname = privateUser.Firstname,
            Lastname = privateUser.Lastname,
            Address = privateUser.Address,
            City = privateUser.City,
            Postcode = privateUser.Postcode,
            DateOfBirth = privateUser.DateOfBirth,
            PrivateUserId = privateUser.Id
        };

        // Legg til Person og i listen under Private User og lagrer Person i tabellen
        _db.Persons.Add(person);
        privateUser.Persons.Add(person);
        await _db.SaveChangesAsync();

        return privateUser;
    }


    public async Task AddPersonToPrivateUser(string privateUserId, Person person)
    {
        // Hent PrivateUser fra databasen
        var privateUser = await _db.PrivateUsers
            .Include(p => p.Persons)
            .FirstOrDefaultAsync(p => p.Id == privateUserId);

        if (privateUser == null)
            throw new Exception("Private User not found");

        //Detter PrivateUSerId på personopbjektet. 
        person.PrivateUserId = privateUserId;
        // Legg til ny Person til Persons-liste og lagre
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

     public async Task<bool> EditUserDetails(ApplicationPrivateUserViewModel viewModel)
    {
        var applicationUser = await _um.Users.FirstOrDefaultAsync(u => u.Id == viewModel.Id);
        var privateUser = await _db.PrivateUsers.FirstOrDefaultAsync(p => p.Id == viewModel.Id);

        if (applicationUser == null || privateUser == null)
        {
            return false;
        }
        applicationUser.Email = viewModel.Email;
        var result = await _um.UpdateAsync(applicationUser);
        if (!result.Succeeded)
        {
            return false;
        }
        
        privateUser.Telephone = viewModel.Telephone; 
        privateUser.Firstname = viewModel.Firstname;
        privateUser.Lastname= viewModel.Lastname;
        privateUser.Address= viewModel.Address;
        privateUser.City= viewModel.City;
        privateUser.Postcode= viewModel.Postcode;
        privateUser.DateOfBirth= viewModel.DateOfBirth;
        
        _db.PrivateUsers.Update(privateUser);
        await _db.SaveChangesAsync();

        return true;
    }
}
