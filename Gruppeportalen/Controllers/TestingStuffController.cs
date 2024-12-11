using Gruppeportalen.Areas.PrivateUser.Models.MembershipsAndPayment;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Controllers;

public class TestingStuffController : Controller
{

    private readonly IMembershipService _ms;
    private readonly IPaymentService _ps;
   
    public TestingStuffController(IMembershipService ms, IPaymentService ps)
    {
        _ms = ms;
        _ps = ps;
    }
    
    public IActionResult Index()
    {
        /*var mtId = new Guid("BEDB0142-6D1E-43EE-9F6D-A9D0958C8BE6");
        var personId = "5e616af2-75f1-4167-9bfb-b77e0e3cc134";
        var lgId = new Guid("A4A04F60-991D-469D-99A4-0B6625E0C4D5");
        var result = _ms.AddMembershipToDatabase(mtId, personId, lgId);*/
        
        var payment = new Payment
        {
            Amount = 150,
        };
        
        var result = _ps.AddPayment(payment);

        if (!result.Result)
        {
            return View(result);
        }
        
        return View(result);
    }

    public IActionResult Delete()
    {
        var result2 = _ps.RemovePaymentById(new Guid("90062CB0-7D33-459F-9B80-2E58346948AC"));

        if (!result2.Result)
        {
            return View(result2);
        }
        
        return View(result2);
        
    }
}