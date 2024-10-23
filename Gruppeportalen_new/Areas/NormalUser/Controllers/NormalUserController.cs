using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen_new.Areas.NormalUser.Controllers;

public class NormalUserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public NormalUserController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }
    public IActionResult Index()
    {
        return View();
    }

    [Authorize, HttpGet]
    public IActionResult Add()
    {
        return View(new Models.NormalUser()); 
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Add(Models.NormalUser normalUser)
    {
        var user = _um.GetUserAsync(User).Result;
        normalUser.Id = user.Id;
        
        _db.NormalUsers.Add(normalUser);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

}