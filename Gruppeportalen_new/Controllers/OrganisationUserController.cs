using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen_new.Controllers;

public class OrganisationUserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public OrganisationUserController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }
    
    [Authorize]
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        
        // Can change this to redirect to an error page maybe?
        if (user.TypeOfUser == "NormalUser")
            return RedirectToAction(nameof(Index), nameof(NormalUser));
        
        if (_db.OrganisationUsers.Find(user.Id) == null)
            return RedirectToAction(nameof(Add));
        return View();
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Add()
    {
        var user = _um.GetUserAsync(User).Result;
        return View(new OrganisationUser {UserId = user.Id}); 
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Add(OrganisationUser organisationUser)
    {
        
        if (!ModelState.IsValid)
        { 
            return View(organisationUser);
        }
        
        _db.OrganisationUsers.Add(organisationUser);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}