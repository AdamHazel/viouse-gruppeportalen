using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen_new.Controllers;

public class NormalUserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public NormalUserController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }

    [Authorize]
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;

        // Can change this to redirect to an error page maybe?
        if (user.TypeOfUser == "CentralOrganisation")
            return RedirectToAction(nameof(Index), nameof(OrganisationUser));
        
        if (_db.NormalUsers.Find(user.Id) == null)
            return RedirectToAction(nameof(Add));
        
        return View();
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult Add()
    {
        var user = _um.GetUserAsync(User).Result;
        
        return View(new NormalUser{Id = user.Id}); 
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Add(NormalUser normalUser)
    {
        if (!ModelState.IsValid)
        { 
            return View(normalUser);
        }
        
        _db.NormalUsers.Add(normalUser);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}