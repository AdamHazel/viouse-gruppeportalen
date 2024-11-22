using System.Security.Claims;
using Gruppeportalen.Areas.PrivateUser.HelperClasses;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
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

public class HomeController : Controller
{
    private readonly UserManager<ApplicationUser> _um;
    
    public HomeController(UserManager<ApplicationUser> um)
    {
        _um = um;
        
    }   
    
    public IActionResult Index()
    {
        return View();
    }

    
}