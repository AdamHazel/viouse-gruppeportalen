using Gruppeportalen.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Controllers;

public class OopsieController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Admin()
    {
        return View();
    }

    public IActionResult ErrorMessage(string message)
    {
        var model = new ErrorMessage
        {
            Message = message
        };
        
        return View(model);
    }
}