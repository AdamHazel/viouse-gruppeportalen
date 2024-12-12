using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    private readonly IGenerateMemberList _gml;
    private readonly IMembershipService _ms;

    public MyLocalGroupsController(UserManager<ApplicationUser> um, IOverviewService os, ILocalGroupService lgs
        , IMembershipTypeService mts, INorwayCountryInformation norwayInfo, IGenerateMemberList gml, IMembershipService ms)
    {
        _um = um;
        _os = os;
        _lgs = lgs;
        _mts = mts;
        _norwayInfo = norwayInfo;
        _gml = gml;
        _ms = ms;
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
    
    [Route("normal/group/{groupId:guid}/overview")]
    [AdminForThisGroupCheckFactory]
    public IActionResult LocalGroupOverview(Guid groupId)
    {
        var group = _lgs.GetLocalGroupById(groupId);
        if (group == null)
            return NotFound("Group not found");
        
        return View(group);
    }

    [Route("normal/group/{groupId:guid}/information")]
    [AdminForThisGroupCheckFactory]
    public IActionResult LocalGroupInformation(Guid groupId)
    { 
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        var group = _lgs.GetLocalGroupById(groupId);
        if (group == null)
            return NotFound("Group not found");
        
        return View(group);
    }

    [HttpGet]
    public IActionResult ExportActiveMembershipsToCsv(Guid groupId)
    {
     
        var csvBytes = _gml.GenerateActiveMembershipsCsv(groupId);
        var fileName = $"AktiveMedlemskap_{DateTime.Now:yyyy.MM}.csv";

        return File(csvBytes, "application/octet-stream", fileName);
    }

    
    [HttpGet]
    public IActionResult CheckIfMembershipListIsEmpty(Guid groupId)
    {
        var isEmpty = _gml.IsMembershipListEmpty(groupId); 
        return Json(new { isEmpty }); 
    }
    

    [HttpPost]
    public IActionResult BlockMember([FromBody] List<Guid> memberIds)
    {
        if (memberIds == null || !memberIds.Any())
        {
            return BadRequest("No member IDs provided.");
        }

        var result = _ms.BlockMembershipById(memberIds);
        if (result)
        {
            return Json(new { success = true, message = "Members blocked successfully." });
        }
        else
        {
            return StatusCode(500, new { success = false, message = "Failed to block members." });
        }

    }

    [HttpPost]
    public IActionResult UnblockMember([FromBody] List<Guid> memberIds)
    {
        if (memberIds == null || !memberIds.Any())
        {
            return BadRequest("No member IDs provided.");
        }

        var result = _ms.UnblockMembershipById(memberIds);
        if (result)
        {
            return Json(new { success = true, message = "Members unblocked successfully." });
        }
        else
        {
            return StatusCode(500, new { success = false, message = "Failed to unblock members." });
        }
    }
}