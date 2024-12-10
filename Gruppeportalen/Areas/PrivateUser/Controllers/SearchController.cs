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
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Controllers;
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
    private readonly INorwayCountryInformation _nci;
    private readonly ILocalGroupService _lgs;
    
    public SearchController(ApplicationDbContext db, UserManager<ApplicationUser> um, 
        IPrivateUserOperations privateUserOperations, INorwayCountryInformation nci, ILocalGroupService lgs)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
        _nci = nci;
        _lgs = lgs;
    }   
    
 public IActionResult Index()
    {
        ViewBag.Counties = _nci.GetAllCounties();
      
        
        var groups = _privateUserOperations.GetAllLocalActiveGroups();
        return View(groups);
    }


    [HttpGet]
    public IActionResult SearchLocalGroups(string query, string county)
    {
        var allGroups = _privateUserOperations.SearchLocalGroups(query, county);
        return PartialView("_LocalGroupCardList", allGroups);
    }
    
    [HttpGet]
    [Route("PrivateUser/Search/{groupId:guid}/addMember")]
    public IActionResult AddMembership(Guid groupId)
    {
        var currentUser = _um.GetUserAsync(User).Result;
        var privateUser = _privateUserOperations.GetPrivateUserById(currentUser.Id);
        var localGroup = _lgs.GetLocalGroupById(groupId);

        if (currentUser == null || privateUser == null)
        {
            return PartialView("ErrorPartialMember");
        }

        if (localGroup == null)
        {
            return PartialView("ErrorPartialMember");
        }

        var userLocalGroup = new PrivateUserLocalGroup
        {
            User = privateUser,
            Group = localGroup,
        };
        return PartialView("_BecomeAMember", userLocalGroup);
    }

    /*[HttpPost]
    public IActionResult AddMembership(Guid membershipTypeChoice, string personChoice)
    {
        
    }*/

    
}
