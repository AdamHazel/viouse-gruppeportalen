using Gruppeportalen.Areas.CentralOrganisation.HelperClasses;
using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.CentralOrganisation.Models.ViewModels;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Controllers;

[Area("CentralOrganisation")]
[Authorize]
[CentralOrgUserCheckFactory]
public class LocalgroupController : Controller
{
    private readonly UserManager<ApplicationUser> _um;
    private readonly ILocalGroupService _lgs;
    private readonly ILocalGroupAdminService _lgas;
    private readonly IOverviewService _overviewService;
    private readonly ICentralOrganisationService _cos;
    private readonly INorwayCountryInformation _norwayInfo;


    public LocalgroupController(UserManager<ApplicationUser> um, ILocalGroupService lgs, ILocalGroupAdminService adminService, 
                                IOverviewService overviewService, ICentralOrganisationService cos, INorwayCountryInformation norwayInfo)
    {
        _um = um;
        _lgs = lgs;
        _lgas = adminService;
        _overviewService = overviewService;
        _cos = cos;
        _norwayInfo = norwayInfo;
    }


    public IActionResult Index()
    {
        var organization = _um.GetUserAsync(User).Result;
        var org = _cos.GetCentralOrganisationByUser(organization.Id);
        var groups = org.LocalGroups.ToList();
        return View(groups);
    }
    

    [HttpGet]
    public IActionResult Add()
    
    {
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        return View(new LocalGroup());
    }
    

    [HttpPost]
    public IActionResult Add(LocalGroup group)
    {
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        if (ModelState.IsValid)
        {
            var organization = _um.GetUserAsync(User).Result;

            if (_lgs.AddNewLocalGroup(group, organization.Id))
                return RedirectToAction(nameof(Index));
            else
            {
                return BadRequest("Error happened when using Local group service.");
            }
        }

        return View(group);
    }
    
    [HttpGet]
    public IActionResult Edit(Guid id)
    {
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        var viewModel = new EditLocalGroupViewModel
        {
            LocalGroup = _lgs.GetLocalGroupById(id),
            AdminCreator = new AdminCreator {LocalGroupId = id},
            LocalGroupAdmins = new List<(ApplicationUser, Guid)>(),
        };
        
        if (viewModel.LocalGroup == null)
            return BadRequest("Unable to find local group");

        viewModel.LocalGroupAdmins = _overviewService.GetLocalGroupAdminsByGroup(viewModel.LocalGroup);
        
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(LocalGroup group)
    {
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        if (!ModelState.IsValid)
        {
            var viewModel = new EditLocalGroupViewModel
            {
                LocalGroup = group,
                AdminCreator = new AdminCreator {LocalGroupId = group.Id},
                LocalGroupAdmins = new List<(ApplicationUser, Guid)>(),
            };
            
            viewModel.LocalGroupAdmins = _overviewService.GetLocalGroupAdminsByGroup(viewModel.LocalGroup);
            return View(viewModel);
        }
        
        if(_lgs.UpdateLocalGroup(group))
            return RedirectToAction(nameof(Index));
        else
        {
            return BadRequest("Error happened when updating local group.");
        }
    }

    [HttpPost]
    public IActionResult AddAdmin(AdminCreator adminCreator)
    {
        
        if (adminCreator.LocalGroupId == Guid.Empty)
            return BadRequest("Id empty");
        ViewBag.Counties = _norwayInfo.GetAllCounties();
        
        if (!ModelState.IsValid)
        {
            var viewModel = new EditLocalGroupViewModel
            {
                LocalGroup = _lgs.GetLocalGroupById(adminCreator.LocalGroupId),
                AdminCreator = adminCreator,
                LocalGroupAdmins = new List<(ApplicationUser, Guid)>(),
            };
            
            viewModel.LocalGroupAdmins = _overviewService.GetLocalGroupAdminsByGroup(viewModel.LocalGroup);
            return View(nameof(Edit), viewModel);
        }

        if (_lgas.AddAdminToLocalGroupByEmail(adminCreator.AdminEmail, adminCreator.LocalGroupId))
        {
            return RedirectToAction(nameof(Edit), new {id = adminCreator.LocalGroupId});
        }
        else
        {
            return BadRequest("Error happened when assigning admin to Local group.");
        }
    }
    
    public IActionResult RemoveAdmin(Guid groupId, string? userId)
    {
        if (groupId == Guid.Empty || userId == null)
            return BadRequest("Unknown local group or admin");

        if (_lgas.RemoveAdminById(userId, groupId))
        {
            return RedirectToAction(nameof(Edit), new {id = groupId});
        }
        else
        {
            return BadRequest("Error happened when removing admin from Local group.");
        }
    }
}