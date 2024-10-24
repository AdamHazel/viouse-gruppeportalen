using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Controllers;

[Authorize]
[Area("CentralOrganisation")]
public class AddController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public AddController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }  
    
    [HttpGet]
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        
        return View(new Models.CentralOrganisation {Id = user.Id});
    }

    [HttpPost]
    public IActionResult Index(Models.CentralOrganisation organisation)
    {
        if (!ModelState.IsValid)
        { 
            return View(organisation);
        }
        
        _db.CentralOrganisations.Add(organisation);
        _db.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}