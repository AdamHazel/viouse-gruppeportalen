using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Services.Classes;

public class PersonService : IPersonService
{

    private readonly ApplicationDbContext _db;

    public PersonService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public string GeneratePersonId()
    {
        return Guid.NewGuid().ToString();
    }
    
    private Person? _createPerson(string fName, string lName, string addr, 
        string city, string postcode, DateTime dob)
    {
        var person = new Person
        {
            Firstname = fName,
            Lastname = lName,
            Address = addr,
            City = city,
            Postcode = postcode,
            DateOfBirth = dob,
        };
        
        return person;
    }

    private bool _addPersonToDb(Person person)
    {
        try
        {
            _db.Persons.Add(person);
            if (_db.SaveChanges() > 0)
            {
                return true;
            }
            else
                throw new DbUpdateException("Failed to update group in db");
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public Person? CreatePrimaryPersonByUser(PrivateUser user)
    {
        var person = _createPerson(
            user.Firstname, user.Lastname, user.Address,
            user.City, user.Postcode, user.DateOfBirth);
        
        if (person != null)
        {
            person.Id = user.Id;
        }

        return person;
    }

    public Person? GetPersonById(string personId)
    {
        return _db.Persons
            .Include(upc => upc.UserPersonConnections)
            .FirstOrDefault(p => p.Id == personId);
    }

    public bool AddPersonToDbByPerson(Person person)
    {
        return _addPersonToDb(person);   
    }
    
}