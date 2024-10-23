using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
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
        
        return View();
    }
    
    [ HttpGet]
    public IActionResult Add()
    {
        return View(new NormalUser()); 
    }
    
    
    [HttpPost]
    public IActionResult Add(NormalUser normalUser)
    {
        if (!ModelState.IsValid)
            return View(normalUser);
        
        var user = _um.GetUserAsync(User).Result;
        normalUser.Id = user.Id;
        
        _db.NormalUsers.Add(normalUser);
        _db.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}