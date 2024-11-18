using System.Security.Claims;
using Gruppeportalen.Areas.CentralOrganisation.Services.Interfaces;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.CentralOrganisation.Controllers;
[Area("CentralOrganisation")]
public class CentralOrganisationController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly ICentralOrganisationService _cos;
    
    public CentralOrganisationController(ApplicationDbContext db, ICentralOrganisationService cos)
    {
        _db = db;
        _cos = cos;
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var viewModel = _cos._getCentralOrganisations(userId);
        if (viewModel == null)
            return NotFound();
        
        return View("Index", viewModel);
    }

    [HttpGet]
    public IActionResult Edit()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var viewModel=_cos._getCentralOrganisations(userId);
        if (viewModel == null)
            return NotFound();

        return View("Edit", viewModel);
    }
    
    [HttpPost]
    public IActionResult Edit(Models.CentralOrganisation viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        viewModel.Id = userId;
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        _cos._editOrganisationDetails(viewModel);

        return RedirectToAction("Index", "CentralOrganisation");
    }
}