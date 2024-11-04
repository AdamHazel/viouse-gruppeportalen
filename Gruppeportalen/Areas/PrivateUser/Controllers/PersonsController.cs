using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.Models;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Gruppeportalen.Areas.PrivateUser.Controllers;
[Area("PrivateUser")]
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
       // ModelState.Remove("Id");
       /* if (!ModelState.IsValid)
        {
            return View("Add", person);
        }*/
     
        var privateUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _privateUserOperations.AddPersonToPrivateUser(privateUserId, person);
        return RedirectToAction("Index");

    }

    [HttpGet]
    public IActionResult Edit(string id)
    {
        var person = _privateUserOperations.getPersonDetails(id);
        if (person == null)
            return NotFound();
            
        return View("Edit", person);
    }

    [HttpPost]
    public IActionResult Edit(Person person)
    {
        /*ModelState.Remove("Id");
        if (!ModelState.IsValid)
        {
            return View(person);
        }*/
        
        _privateUserOperations.EditPerson(person);
        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult Delete(string id)
    {
        _privateUserOperations.DeletePerson(id);
        return RedirectToAction("Index");
    }


}