using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

    public MyLocalGroupsController(UserManager<ApplicationUser> um, IOverviewService os, ILocalGroupService lgs)
    {
        _um = um;
        _os = os;
        _lgs = lgs;
        
    }
    
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        var overview = new CompleteLocalGroupOverview
        {
            AdminOverview = _os.GetAdminLocalGroupOverview(user.Id),
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
        var group = _lgs.GetLocalGroupById(groupId);
        if (group == null)
            return NotFound("Group not found");
        
        return View(group);
    }
}