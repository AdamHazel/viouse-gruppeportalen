using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Controllers;

public class OopsieController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}