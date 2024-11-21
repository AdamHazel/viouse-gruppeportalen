using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

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

        if (_db.PrivateUsers.Find(user.Id) == null)
            return RedirectToAction("Index", "Add");
        
        return View();
    }

    
}