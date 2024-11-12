using Gruppeportalen.Areas.CentralOrganisation.HelperClasses;
using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.CentralOrganisation.Models.ViewModels;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Models;
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


    public LocalgroupController(UserManager<ApplicationUser> um, ILocalGroupService lgs, ILocalGroupAdminService adminService)
    {
        _um = um;
        _lgs = lgs;
        _lgas = adminService;
    }


    public IActionResult Index()
    {
        var organization = _um.GetUserAsync(User).Result;
        var groups = _lgs.GetLocalGroups(organization.Id);
        return View(groups);
    }
    

    [HttpGet]
    public IActionResult Add()
    
    {
        return View(new LocalGroup());
    }
    

    [HttpPost]
    public IActionResult Add(LocalGroup group)
    {
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
        var viewModel = new EditLocalGroupViewModel
        {
            LocalGroup = _lgs.GetLocalGroupById(id),
            AdminCreator = new AdminCreator {LocalGroupId = id},
            LocalGroupAdmins = new List<ApplicationUser>(),
        };
        
        if (viewModel.LocalGroup == null)
            return BadRequest("Unable to find local group");

        viewModel.LocalGroupAdmins = _lgas.GetLocalGroupAdminsByGroup(viewModel.LocalGroup);
        
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Edit(LocalGroup group)
    {
        if (!ModelState.IsValid)
        {
            var viewModel = new EditLocalGroupViewModel
            {
                LocalGroup = group,
                AdminCreator = new AdminCreator {LocalGroupId = group.Id},
                LocalGroupAdmins = new List<ApplicationUser>(),
            };
            
            viewModel.LocalGroupAdmins = _lgas.GetLocalGroupAdminsByGroup(viewModel.LocalGroup);
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
        
        if (!ModelState.IsValid)
        {
            var viewModel = new EditLocalGroupViewModel
            {
                LocalGroup = _lgs.GetLocalGroupById(adminCreator.LocalGroupId),
                AdminCreator = adminCreator,
                LocalGroupAdmins = new List<ApplicationUser>(),
            };
            
            viewModel.LocalGroupAdmins = _lgas.GetLocalGroupAdminsByGroup(viewModel.LocalGroup);
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
}