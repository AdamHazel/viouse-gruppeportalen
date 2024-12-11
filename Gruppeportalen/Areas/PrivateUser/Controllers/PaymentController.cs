using Braintree;
using Gruppeportalen.Areas.PrivateUser.Models.ViewModels;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gruppeportalen.Areas.PrivateUser.Controllers;

[Area("PrivateUser")]
public class PaymentController : Controller
{
    private readonly IBraintreeService _braintreeService;
    private readonly ApplicationDbContext _db;
    private readonly IPaymentService _pay;
    private readonly IMembershipService _ms;
    private readonly IUserService _userService;
    private readonly IPaymentService _paymentService;
    

    public PaymentController(
        IBraintreeService braintreeService, 
        ApplicationDbContext db, 
        IPaymentService pay, 
        IMembershipService ms,
        IUserService userService,
        IPaymentService paymentService
        )
    {
        _braintreeService = braintreeService;
        _db = db;
        _pay = pay;
        _ms = ms;
        _userService = userService;
        _paymentService = paymentService;
    }
    
    // GET
    public async Task<IActionResult> Checkout(Guid membershipId, Guid paymentId)
    {
        var gateway = _braintreeService.GetGateway();
        var clientToken = gateway.ClientToken.Generate();
        ViewBag.ClientToken = clientToken;

        // Fetch the membership using membershipId
        var membership = await _db.Memberships
            .Include(m => m.MembershipType) // Include MembershipType to get the name
            .FirstOrDefaultAsync(m => m.Id == membershipId);
        
        // Fetch the payment using paymentId
        var payment = await _db.Payments
            .FirstOrDefaultAsync(p => p.Id == paymentId);

        // Create the PaymentViewModel
        var paymentViewModel = new PaymentViewModel
        {
            PaymentId = payment.Id,
            MembershipId = membership.Id, // Include MembershipId
            MembershipName = membership.MembershipType.MembershipName,
            Price = payment.Amount // Use the amount from the payment
        };

        return View(paymentViewModel); // Render payment form
    }

    [HttpPost]
    public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Checkout", model);
        }

        var gateway = _braintreeService.GetGateway();
        
        var request = new TransactionRequest
        {
            Amount = (decimal)model.Price, // Payment amount
            PaymentMethodNonce = model.Nonce, // Nonce from Drop-In UI
            Options = new TransactionOptionsRequest
            {
                SubmitForSettlement = true // Automatically settle the transaction
            }
        };

        var result = gateway.Transaction.Sale(request);

        if (result.IsSuccess())
        {
            // Get the current user using IUserService
            var currentUser = await _userService.GetCurrentUserAsync(User);

            // Mark payment as paid
            var paymentUpdated = _pay.MarkPaymentAsPaid(model.PaymentId, currentUser.Id);
            if (!paymentUpdated) return View("Error"); // Handle failure

            // Activate the membership
            var membershipActivated = _ms.ActivateMembership(model.MembershipId);
            if (!membershipActivated) return View("Error"); // Handle failure

            return View("Success");
        }
        else
        {
            Console.WriteLine("Payment failed:");
            Console.WriteLine($"Message: {result.Message}");
            foreach (var error in result.Errors.DeepAll())
            {
                Console.WriteLine($"Error: {error.Code} - {error.Message}");
            }

            model.ErrorMessage = result.Message;
            return View("Checkout", model); // Show error message on the same page
        }
    }
}