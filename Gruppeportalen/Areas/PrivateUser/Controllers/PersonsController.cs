using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Gruppeportalen.Areas.PrivateUser.Controllers;


[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

public class PersonsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly IPrivateUserOperations _puo;
    private readonly IUserPersonConnectionsService _upc;
    private readonly IPersonService _ps;

    public PersonsController(ApplicationDbContext db, UserManager<ApplicationUser> um,
        IPrivateUserOperations puo, IUserPersonConnectionsService upc, IPersonService ps)
    {
        _db = db;
        _um = um;
        _puo = puo;
        _upc = upc;
        _ps = ps;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var currentUser = _um.GetUserAsync(User).Result;
        var privateUser = _puo.GetPrivateUserByIdWithConnectedPersons(currentUser.Id);
        var specialView = new PrimaryPersonAndOthers
        {
            PrimaryPerson = new Person(),
            OtherPersons = new List<Person>(),
        };
        foreach (var connection in privateUser.UserPersonConnections)
        {
            if (connection.PersonId == connection.PrivateUserId)
            {
                specialView.PrimaryPerson = connection.Person;
                specialView.PrimaryPerson.PrimaryPerson = true;
            }
            else
            {
                connection.Person.PrimaryPerson = false;
                specialView.OtherPersons.Add(connection.Person);
            }
        }
        return View(specialView);

        /*var privateUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var privateUser = _db.PrivateUsers
            .Include(u => u.Persons)
            .FirstOrDefault(u => u.Id == privateUserId);

        if (privateUser == null)
        {
            return NotFound();
        }
        var persons = _privateUserOperations.GetAllPersons(privateUserId);

        return View(persons);*/
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View(new Person());
    }
    

    [HttpPost]
    public IActionResult Add(Person person)
    {
        if (ModelState.IsValid)
        {
            var privateUser = _um.GetUserAsync(User).Result;

            if (_puo.AddPersonToPrivateUser(privateUser.Id, person)) 
                return RedirectToAction(nameof(Index));

            else 
            { 
                return BadRequest("Error happened when using Private User services"); 
            } 
        } 
        return View(person); 
    }

[HttpGet]
    public IActionResult Edit(string id)
    {
        var person = _puo.GetPersonDetails(id);
        if (person == null)
            return NotFound();
            
        return View("Edit", person);
    }

    [HttpPost]
    public IActionResult Edit(Person person)
    {
        if (!ModelState.IsValid) return View("Edit");
        _puo.EditPerson(person);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(string personId)
    {
        /*_puo.DeletePerson(id);*/
        var currentUser = _um.GetUserAsync(User).Result;
        
        var resultOfRemoval = _upc.DeleteUserPersonConnection(currentUser.Id, personId);
        if (resultOfRemoval.Result)
        {
            var otherConnections = _upc.DoesPersonHaveOtherConnections(personId);
            if (otherConnections)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (_ps.RemovePersonFromDbById(personId))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction("ErrorMessage", "Oopsie", 
                        new { Area = "", message = "Unable to remove person from DB" });
                }
            }
        }
        else
        {
            return RedirectToAction("ErrorMessage", "Oopsie", 
                new { Area = "", message = resultOfRemoval.Message });
        }
    }

    [HttpPost]
    public IActionResult SharePerson(string email, string personId)
    {
        if (_db.Users.FirstOrDefault(u => u.Email == email) == null)
        {
            return BadRequest("Beklager. Brukeren med oppgitt epost adresse eksisterer ikke i systemet, kunne ikke dele person.");
        }
        _puo.SharePersonWithUser(email, personId);
        return RedirectToAction("Index");
    }



    [HttpPost]
    public IActionResult TransferPerson(string email, string personId)
    {
        if (_db.Users.FirstOrDefault(u => u.Email == email) == null)
        {
            return BadRequest(
                "Beklager. Brukeren med oppgitt epost adresse eksisterer ikke i systemet, kunne ikke overføre person.");
        }

        _puo.TransferPerson(email, personId);
        return RedirectToAction("Index");
    }
    
}