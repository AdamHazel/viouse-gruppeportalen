using Gruppeportalen.Areas.CentralOrganisation.HelperClasses;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Controllers;

[Area("CentralOrganisation")]
[Authorize]
[CentralOrgUserCheckFactory]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }
    
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        
        if (_db.CentralOrganisations.Find(user.Id) == null)
            return RedirectToAction("Index", "Add");
        
        return View();
    }
}