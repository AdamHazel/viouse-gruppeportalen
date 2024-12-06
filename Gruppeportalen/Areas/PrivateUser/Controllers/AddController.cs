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
    private readonly IPersonService _ps;
    private readonly IUserPersonConnectionsService _upc;
    
    public AddController(ApplicationDbContext db, UserManager<ApplicationUser> um, 
        IPrivateUserOperations privateUserOperations, IPersonService ps, IUserPersonConnectionsService upc)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
        _ps = ps;
        _upc = upc;
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
            var person = _ps.CreatePrimaryPersonByUser(privateUser);
            if (person == null)
            {
                return RedirectToAction("Index", "Oopsie", new { Area = "" });
            }
            else
            {
                var resultOfAddingPerson = _ps.AddPersonToDbByPerson(person);
                var resultOfAddingConnection = _upc.AddUserPersonConnection(privateUser.Id, person.Id);

                if (resultOfAddingPerson == false || resultOfAddingConnection == false)
                {
                    return RedirectToAction("Index", "Oopsie", new { Area = "" });
                }
            }
        }
        
        return RedirectToAction("Index", "Home");
    }

   
}