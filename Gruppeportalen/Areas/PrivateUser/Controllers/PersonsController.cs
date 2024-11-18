using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

public class PersonsController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly PrivateUserOperations _privateUserOperations;
    
    public PersonsController(ApplicationDbContext db, UserManager<ApplicationUser> um, PrivateUserOperations privateUserOperations)
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

        return View(privateUser.Persons);
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View(new Person());
    }

    [HttpPost]
    public IActionResult Add(Person person)
    {
        if (!ModelState.IsValid)
        {
            var privateUser=_um.GetUserAsync(User).Result;
           
            if(_privateUserOperations.AddPersonToPrivateUser(privateUser.Id, person));
            return RedirectToAction(nameof(System.Index));
        }
        else
        {
            return BadRequest("Error happened when using Private User services");
        }
      
        return RedirectToAction("Index");
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
        if (!ModelState.IsValid)
        {
            return View(person);
        }
        _privateUserOperations.EditPerson(person);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(Guid id)
    {
        _privateUserOperations.DeletePerson(id);
        return RedirectToAction("Index");
    }
}