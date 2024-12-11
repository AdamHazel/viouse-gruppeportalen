using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

public class MyLocalGroupsController : Controller
{
    private readonly UserManager<ApplicationUser> _um;
    private readonly IOverviewService _os;
    private readonly ILocalGroupService _lgs;
    private readonly INorwayCountryInformation _norwayInfo;
    private readonly IMembershipTypeService _mts;

    public MyLocalGroupsController(UserManager<ApplicationUser> um, IOverviewService os, ILocalGroupService lgs
        , IMembershipTypeService mts, INorwayCountryInformation norwayInfo)
    {
        _um = um;
        _os = os;
        _lgs = lgs;
        _mts = mts;
        _norwayInfo = norwayInfo;
    }
    
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        var overview = new CompleteLocalGroupOverview
        {
            AdminOverview = _os.GetAdminLocalGroupOverview(user.Id),
            PersonOverview = _os.GetPersonLocalGroupOverview(user.Id),
        };
        
        return View(overview);
    }
    
    [Route("admin/group/{groupId:guid}/overview")]
    [AdminForThisGroupCheckFactory]
    public IActionResult AdminGroupOverview(Guid groupId)
    {
        var group = _lgs.GetLocalGroupById(groupId);
        if (group == null)
            return NotFound("Group not found");
        
        return View(group);
    }

    [Route("admin/group/{groupId:guid}/information")]
    [AdminForThisGroupCheckFactory]
    public IActionResult AdminGroupInformation(Guid groupId)
    { 
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        var group = _lgs.GetLocalGroupById(groupId);
        if (group == null)
            return NotFound("Group not found");
        
        return View(group);
    }
    
    [HttpPost]
    public IActionResult Edit(LocalGroup viewModel)
    {
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        var group = _lgs.GetLocalGroupById(viewModel.Id);
        if (group == null)
        {
            return NotFound();
        }
        if (!ModelState.IsValid)
        {
            return View("AdminGroupInformation", viewModel);
        }
        _lgs.UpdateLocalGroupAsAdmin(viewModel);
        return View("AdminGroupInformation", group);
    }
    
    [HttpPost]
    public IActionResult AddMembershipType(MembershipType model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        var success = _mts.AddNewMembershipType(model, model.LocalGroupId);
        
        if (!success)
        {
            return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });
        }
        return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });
    }

    [HttpPost]
    public IActionResult EditMembershipType(MembershipType model)
    {
        if (!ModelState.IsValid)
        {

            return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });
        }

        var success = _mts.UpdateMembershipType(model);
        if (success)
            return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });

        else return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });
    }

    [HttpPost]
    public IActionResult DeleteMembershipType(Guid id)
    {
        var model= _mts.GetMembershipTypeById(id);
        if (model == null)
        {
            return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });
        }
        _mts.DeleteMembershipType(id);
        return RedirectToAction("AdminGroupInformation", new { groupId = model.LocalGroupId });
    }

    
    public IActionResult AdminGroupMembers(Guid groupId)
    {
        var group = _lgs.GetLocalGroupById(groupId);
        return View("AdminGroupMembers", group);
    }
}