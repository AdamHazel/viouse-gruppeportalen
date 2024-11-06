using System.Security.Claims;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
public class AddController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly PrivateUserOperations _privateUserOperations;
    
    public AddController(ApplicationDbContext db, UserManager<ApplicationUser> um, PrivateUserOperations privateUserOperations)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;;
        
        return View(new Models.PrivateUser { Id = user.Id });
    }
    
    
    [HttpPost]
    public IActionResult Index(Models.PrivateUser privateUser)
    {
        
        if (!ModelState.IsValid)
        { 
            return View(privateUser);
        }
        
        _privateUserOperations.CreatePrivateUserWithPerson(privateUser);
        
        return RedirectToAction("Index", "Home");
    }

   
}