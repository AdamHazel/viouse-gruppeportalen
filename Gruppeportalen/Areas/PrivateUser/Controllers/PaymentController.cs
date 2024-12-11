using Braintree;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Area("PrivateUser")]
public class PaymentController : Controller
{
    private readonly IBraintreeService _braintreeService;
    private readonly ApplicationDbContext _db;

    public PaymentController(IBraintreeService braintreeService, ApplicationDbContext db)
    {
        _braintreeService = braintreeService;
        _db = db;
    }
    
    // GET
    public IActionResult Checkout(Guid membershipTypeId)
    {
        var gateway = _braintreeService.GetGateway();
        var clientToken = gateway.ClientToken.Generate();
        ViewBag.ClientToken = clientToken;
        
        // Fetch membership details (e.g., price, name) based on membershipTypeId
        var membershipDetails = _db.MembershipTypes
            .Where(m => m.Id == membershipTypeId)
            .Select(m => new PaymentViewModel
            {
                MembershipTypeId = m.Id,
                MembershipName = m.MembershipName,
                Price = m.Price
            })
            .FirstOrDefault();

        if (membershipDetails == null)
        {
            return NotFound("Membership not found");
        }

        return View(membershipDetails);
    }

    [HttpPost]
    public IActionResult ProcessPayment(PaymentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Checkout", model);
        }

        var gateway = _braintreeService.GetGateway();
        
        var request = new TransactionRequest
        {
            Amount = model.Price, // Payment amount
            PaymentMethodNonce = model.Nonce, // Nonce from Drop-In UI
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true // Automatically settle the transaction
            }
        };

        var result = gateway.Transaction.Sale(request);

        if (result.IsSuccess())
        {
            // TODO: Save payment details to the database
            return View("Success");
        }
        else
        {
            model.ErrorMessage = result.Message;
            return View("Checkout", model); // Show error message
        }
    }
}