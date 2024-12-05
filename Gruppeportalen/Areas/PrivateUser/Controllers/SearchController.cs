using System.Security.Claims;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Services.Interfaces;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

public class SearchController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly IPrivateUserOperations _privateUserOperations;
    public SearchController(ApplicationDbContext db, UserManager<ApplicationUser> um, IPrivateUserOperations privateUserOperations)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
    }   
    

    
    
 public IActionResult Index()
    {
        ViewBag.Counties = _privateUserOperations.GetAllCounties();
      
        
        var groups = _privateUserOperations.GetAllLocalGroups();
        return View(groups);
    }


    [HttpGet]
    public IActionResult SearchLocalGroups(string query, string county)
    {
        var allGroups = _privateUserOperations.SearchLocalGroups(query, county);
        return PartialView("_LocalGroupCardList", allGroups);
    }

    
}
