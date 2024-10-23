using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen_new.Areas.NormalUser.Controllers;

[Area("NormalUser")]
public class AddController : Controller
{

    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public AddController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }


    [ HttpGet]
    public IActionResult Index()
    {
        return View(new Gruppeportalen_new.Areas.NormalUser.Models.NormalUser()); 
    }
    
    
    [HttpPost]
    public IActionResult Index(Gruppeportalen_new.Areas.NormalUser.Models.NormalUser normalUser)
    {
        var user = _um.GetUserAsync(User).Result;
        normalUser.Id = user.Id;
        
        _db.NormalUsers.Add(normalUser);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

}