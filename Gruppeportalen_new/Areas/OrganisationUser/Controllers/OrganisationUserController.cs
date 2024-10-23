using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen_new.Areas.OrganisationUser.Controllers;

public class OrganisationUserController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    public OrganisationUserController(ApplicationDbContext db, UserManager<ApplicationUser> um)
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
        return View(new Models.OrganisationUser()); 
    }
    
    [Authorize]
    [HttpPost]
    public IActionResult Add(Models.OrganisationUser organisationUser)
    {
        var user = _um.GetUserAsync(User).Result;
        organisationUser.Id = user.Id;
        
        _db.OrganisationUsers.Add(organisationUser);
        _db.SaveChanges();

        return RedirectToAction("Index");
    }

}