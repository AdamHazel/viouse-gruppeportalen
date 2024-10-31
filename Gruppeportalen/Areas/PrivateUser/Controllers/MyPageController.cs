using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

public class MyPageController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}