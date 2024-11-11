using Gruppeportalen.Areas.CentralOrganisation.HelperClasses;
using Gruppeportalen.Areas.CentralOrganisation.Models;
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
        var lg = _lgs.GetLocalGroupById(id);
        if (lg == null)
            return BadRequest("Unable to find local group");
        return View(lg);
    }

    [HttpPost]
    public IActionResult Edit(LocalGroup group)
    {
        if (!ModelState.IsValid)
        {
           return View(group);
        }
        
        if(_lgs.UpdateLocalGroup(group))
            return RedirectToAction(nameof(Index));
        else
        {
            return BadRequest("Error happened when updating local group.");
        }
    }

    [HttpGet]
    public IActionResult AddAdmin(Guid id)
    {
        return View(new AdminCreator {LocalGroupId = id});
    }

    [HttpPost]
    public IActionResult AddAdmin(AdminCreator adminCreator)
    {
        
        if (adminCreator.LocalGroupId == Guid.Empty)
            return BadRequest("Id empty");
        
        if (ModelState.IsValid)
        {
            if (_lgas.AddAdminToLocalGroupByEmail(adminCreator.AdminEmail, adminCreator.LocalGroupId))
                return RedirectToAction(nameof(Index));
            else
            {
                return BadRequest("Error happened when assigning admin to Local group.");
            }
        }
        
        return View(adminCreator);
    }
}