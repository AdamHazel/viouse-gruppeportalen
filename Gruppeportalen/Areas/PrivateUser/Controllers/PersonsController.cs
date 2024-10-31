using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

public class PersonsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}