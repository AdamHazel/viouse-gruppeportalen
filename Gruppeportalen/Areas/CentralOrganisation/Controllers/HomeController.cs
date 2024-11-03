using Gruppeportalen.Areas.CentralOrganisation.HelperClasses;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Controllers;

[Area("CentralOrganisation")]
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
    
    [Authorize]
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        
        /*// Can change this to redirect to an error page maybe?
        if (user.TypeOfUser == "PrivateUser")
            return RedirectToAction("Index", "Home", new { area = nameof(PrivateUser)});*/

        if (_db.CentralOrganisations.Find(user.Id) == null)
            return RedirectToAction("Index", "Add");
        
        return View();
    }
}