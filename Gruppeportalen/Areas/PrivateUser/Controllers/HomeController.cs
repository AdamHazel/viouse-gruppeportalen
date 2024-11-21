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
    private readonly IOverviewService _os;
    
    public HomeController(UserManager<ApplicationUser> um, IOverviewService os)
    {
        _um = um;
        _os = os;
    }   
    
    public IActionResult Index()
    {
        var user = _um.GetUserAsync(User).Result;
        var overview = new CompleteLocalGroupOverview
        {
            UserOverview = _os.GetUserLocalGroupOverview(user.Id),
        };
        
        return View(overview);
    }

    
}