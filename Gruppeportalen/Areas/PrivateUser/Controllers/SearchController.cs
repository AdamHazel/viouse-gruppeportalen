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
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
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
    private readonly IMembershipService _ms;
    private readonly IPaymentService _pay;
    
    public SearchController(ApplicationDbContext db, UserManager<ApplicationUser> um, 
        IPrivateUserOperations privateUserOperations, INorwayCountryInformation nci, 
        ILocalGroupService lgs, IMembershipService ms, IPaymentService pay)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
        _nci = nci;
        _lgs = lgs;
        _ms = ms;
        _pay = pay;
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

    [HttpPost]
    [Route("PrivateUser/Search/{groupId:guid}/addMember")]
    public IActionResult AddMembership(Guid localGroupId, Guid membershipTypeChoice, string personChoice)
    {
        var allowedToAdd = _ms.AllowedToAddMembership(membershipTypeChoice, personChoice, localGroupId);
        if (allowedToAdd == null)
        {
            return Json(new { success = false, message = "Det er ikke mulig å sjekke om du kan legge til dette medlemsskapet." });
        }

        if (!allowedToAdd.Result)
        {
            return Json(new { success = false, message = allowedToAdd.Message });
        }
        
        var result = _ms.AddMembershipToDatabase(membershipTypeChoice, personChoice, localGroupId);
        if (result == null)
        {
            return Json(new { success = false, message = "Adding membership returned null" });
        }

        if (!result.R.Result)
        {
            return Json(new { success = false, message = result.R.Message });
        }

        var payment = new Payment
        {
            Amount = result.M.MembershipType.Price,
        };
        
        var resultOfAddingPayment = _pay.AddPayment(payment);
        if (resultOfAddingPayment == null)
        {
            var r = _ms.RemoveMembershipById(result.M.Id);
            return Json(new { success = false, message = "Adding payment returned null" });
        }

        if (!resultOfAddingPayment.Result)
        {
            var r = _ms.RemoveMembershipById(result.M.Id);
            return Json(new { success = false, message = resultOfAddingPayment.Message });
        }

        var resultOfAddingMemberPayment = _pay.AddMemberPayment(payment.Id, result.M.Id);
        if (resultOfAddingMemberPayment == null)
        {
            var r = _ms.RemoveMembershipById(result.M.Id);
            var r2 = _pay.RemovePaymentById(payment.Id);
            return Json(new { success = false, message = "Adding memberpayment returned null" });
        }

        if (!resultOfAddingMemberPayment.Result)
        {
            var r = _ms.RemoveMembershipById(result.M.Id);
            var r2 = _pay.RemovePaymentById(payment.Id);
            return Json(new { success = false, message = resultOfAddingPayment.Message });
        }
        
        return Json(new {success = true});
    }

    
}
