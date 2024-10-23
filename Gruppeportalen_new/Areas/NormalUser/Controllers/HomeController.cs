using Gruppeportalen_new.Data;
using Gruppeportalen_new.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen_new.Areas.NormalUser.Controllers;
[Area("NormalUser")]
public class HomeController : Controller

{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<ApplicationUser> _um;

    public HomeController(ApplicationDbContext db, UserManager<ApplicationUser> um)
    {
        _db = db;
        _um = um;
    }

    public IActionResult Index()
    {
        return View();
    }
}