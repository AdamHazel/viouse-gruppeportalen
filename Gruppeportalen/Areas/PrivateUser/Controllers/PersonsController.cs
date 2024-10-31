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
    public async Task<IActionResult> Index()
    {
        var privateUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
        var privateUser = await _db.PrivateUsers
            .Include(u => u.Persons)
            .FirstOrDefaultAsync(u => u.Id == privateUserId);

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
    public async Task<IActionResult> Add(Person person)
    {
      /*  if (!ModelState.IsValid)
        {
            return View("Add", person);
        }*/
     
        var privateUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _privateUserOperations.AddPersonToPrivateUser(privateUserId, person);
        return RedirectToAction("Index");

    }
}