using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Models.ViewModels;
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
                throw new DbUpdateException("Failed to add person to db");
        }
        catch (DbUpdateException e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    private bool _removePersonFromDb(Person person)
    {
        try
        {
            _db.Persons.Remove(person);
            if (_db.SaveChanges() > 0)
            {
                return true;
            }
            else
                throw new DbUpdateException("Failed to remove person from db");
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
        var person = _db.Persons
                         .Include (m => m.Memberships)
                            .ThenInclude (m => m.LocalGroup)
                        .Include (m => m.Memberships)
                            .ThenInclude (m => m.MembershipType)
                         .Include(upc => upc.UserPersonConnections)
                         .FirstOrDefault(p => p.Id == personId);

        if (person != null)
        {
            person.Memberships = person.Memberships.OrderBy(m => m.LocalGroup.GroupName).ToList();
        }

        return person;
    }

    public ResultOfOperation AddPersonToDbByPerson(Person person)
    {
        var result = new ResultOfOperation
        {
            Result = false,
            Message = String.Empty,
        };
        result.Result = _addPersonToDb(person);
        if (!result.Result)
        {
            result.Message = "Failed to add person to db. Please contact administrator";
        }
        
        return result;   
    }

    public bool RemovePersonFromDbById(string personId)
    {
        var person = GetPersonById(personId);
        if (person != null)
            return _removePersonFromDb(person);
        else return false;
    }
    
}