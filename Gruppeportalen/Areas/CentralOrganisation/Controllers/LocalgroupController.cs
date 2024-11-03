using Gruppeportalen.Areas.CentralOrganisation.Models;
using Gruppeportalen.Areas.CentralOrganisation.Services.Classes;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Controllers;

[Area("CentralOrganisation")]
[Authorize]
public class LocalgroupController : Controller
{
    private readonly UserManager<ApplicationUser> _um;
    private readonly ILocalGroupService _lgs;
    private readonly IIdGeneratorService _idGenerator;

    public LocalgroupController(UserManager<ApplicationUser> um, ILocalGroupService lgs, IIdGeneratorService idGenerator)
    {
        _um = um;
        _lgs = lgs;
        _idGenerator = idGenerator;
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
        return View(new LocalGroup() );
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
}