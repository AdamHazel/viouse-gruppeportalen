using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
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
        
        return View(new Models.PrivateUser {Id = user.Id});
    }
    
    [HttpPost]
    public IActionResult Index(Models.PrivateUser privateUser)
    {
        if (!ModelState.IsValid)
        { 
            return View(privateUser);
        }
        
        _db.PrivateUsers.Add(privateUser);
        _db.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
}