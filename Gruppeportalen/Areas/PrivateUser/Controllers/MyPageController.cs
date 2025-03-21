using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Authorize]
[Area("PrivateUser")]
[PrivateUserCheckFactory]
[PrivateUserInformationCheckFactory]

public class MyPageController : Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;
    private readonly IPrivateUserOperations _privateUserOperations;

    public MyPageController(ApplicationDbContext db, UserManager<ApplicationUser> um,
        IPrivateUserOperations privateUserOperations)
    {
        _db = db;
        _um = um;
        _privateUserOperations = privateUserOperations;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var viewModel = _privateUserOperations.GetUserDetails(userId);

        if (viewModel == null)
            return NotFound();

        return View("Index", viewModel);
    }

    [HttpGet]
    public IActionResult Edit()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var viewModel = _privateUserOperations.GetUserDetails(userId);
        if (viewModel == null)
            return NotFound();

        return View("Edit", viewModel);
    }

    [HttpPost]
    public IActionResult Edit(ApplicationPrivateUser viewModel)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        viewModel.Id = userId;

        ModelState.Remove("Id");
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        _privateUserOperations.EditUserDetails(viewModel);
        return RedirectToAction("Index", "MyPage");
    }
}