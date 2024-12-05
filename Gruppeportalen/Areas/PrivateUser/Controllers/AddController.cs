using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]

public class AddController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly IPrivateUserOperations _privateUserOperations;
    
    public AddController(ApplicationDbContext db, UserManager<ApplicationUser> um, IPrivateUserOperations privateUserOperations)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;;
        
        return View(new Gruppeportalen.Models.PrivateUser { Id = user.Id });
    }
    
    
    [HttpPost]
    public IActionResult Index(Gruppeportalen.Models.PrivateUser privateUser)
    {
        
        if (!ModelState.IsValid)
        { 
            return View(privateUser);
        }

        if (_privateUserOperations.AddPrivateUserToDb(privateUser))
        {
            _privateUserOperations.CreatePersonConnectedToPrivateUser(privateUser);
        }
        
        return RedirectToAction("Index", "Home");
    }

   
}