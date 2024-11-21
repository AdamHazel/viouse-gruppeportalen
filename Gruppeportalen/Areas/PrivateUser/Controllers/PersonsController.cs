using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
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
    private readonly IPrivateUserOperations _privateUserOperations;

    public PersonsController(ApplicationDbContext db, UserManager<ApplicationUser> um,
        IPrivateUserOperations privateUserOperations)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var privateUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var privateUser = _db.PrivateUsers
            .Include(u => u.Persons)
            .FirstOrDefault(u => u.Id == privateUserId);

        if (privateUser == null)
        {
            return NotFound();
        }
        var persons = _privateUserOperations.GetAllPersons(privateUserId);

        return View(persons);
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

            if (_privateUserOperations.AddPersonToPrivateUser(privateUser.Id, person)) 
                return RedirectToAction(nameof(Index));

            else 
            { 
                return BadRequest("Error happened when using Private User services"); 
            } 
        } 
        return View(person); 
    }

[HttpGet]
    public IActionResult Edit(Guid id)
    {
        var person = _privateUserOperations.GetPersonDetails(id);
        if (person == null)
            return NotFound();
            
        return View("Edit", person);
    }

    [HttpPost]
    public IActionResult Edit(Person person)
    {
<<<<<<< HEAD
        if (!ModelState.IsValid) return View("Edit");
=======
     /*  ModelState.Remove("Id");
        if (!ModelState.IsValid)
        {
            return View(person);
        }*/
     
>>>>>>> 0676374 (Adding changes to nora-VIOUSE-17 in order that a rebase of this branch can work)
        _privateUserOperations.EditPerson(person);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        _privateUserOperations.DeletePerson(id);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult SharePerson(string email, Guid personId)
    {
        _privateUserOperations.SharePersonWithUser(email, personId);
        return RedirectToAction("Index");
    }



    [HttpPost]
    public IActionResult TransferPerson(string email, Guid personId)
    {
        try
        {
            _privateUserOperations.TransferPerson(email, personId);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"En feil oppstod under overføringen: {ex.Message}");
            TempData["ErrorMessage"] = "Kunne ikke overføre personen.";
            return RedirectToAction("Index");
        }
    }
    
    
}