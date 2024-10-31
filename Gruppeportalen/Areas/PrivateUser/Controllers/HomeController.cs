using System.Security.Claims;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
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
        
        // Can change this to redirect to an error page maybe?
        if (user.TypeOfUser == "CentralOrganisation")
            return RedirectToAction("Index", "Home", new { area = nameof(CentralOrganisation)});

        if (_db.PrivateUsers.Find(user.Id) == null)
            return RedirectToAction("Index", "Add");
        
        return View();
    }

    
}