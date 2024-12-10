using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Gruppeportalen.Models;
using Gruppeportalen.Data;
using Microsoft.AspNetCore.WebUtilities;  
using System.Text;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Gruppeportalen.Areas.Identity.Pages.Account
{
    public class RegisterPartialModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterPartialModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterPartialModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<RegisterPartialModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty] 
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            [Required, EmailAddress]
            public string Email { get; set; } = string.Empty;

            [Required, StringLength(100, MinimumLength = 6)]
            public string Password { get; set; } = string.Empty;

            [Required]
            public string TypeOfUser { get; set; } = string.Empty;
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException("Unable to create user instance.");
            }
        }

       public async Task<IActionResult> OnPostAsync()
{
    try
    {
        // Check if the form data is valid
        if (!ModelState.IsValid)
        {
            return new JsonResult(new
            {
                success = false,
                errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
            });
        }

        // Create the user
        var user = CreateUser();
        await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
        user.TypeOfUser = Input.TypeOfUser;

        // Attempt to save the user
        var result = await _userManager.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            return new JsonResult(new
            {
                success = false,
                errors = result.Errors.Select(e => e.Description).ToList()
            });
        }

        // Generate the email confirmation link
        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var confirmationUrl = Url.Page(
            "/Account/ConfirmEmail",
            pageHandler: null,
            values: new { area = "Identity", userId = userId, code = code },
            protocol: Request.Scheme);

        // Send the email confirmation
        var subject = "Confirm Your Email";
        var body = $"<p>Thank you for registering! Please confirm your email by clicking the link below:</p>" +
                   $"<p><a href=\"{confirmationUrl}\">Confirm Email</a></p>";
        await _emailSender.SendEmailAsync(Input.Email, subject, body);

        // Return success response
        return new JsonResult(new
        {
            success = true,
            userId,
            message = "A confirmation email has been sent!"
        });
    }
    catch (Exception ex)
    {
        // Log the error and return a JSON response
        Console.WriteLine($"Error during registration: {ex.Message}");

        return new JsonResult(new
        {
            success = false,
            errors = new[] { "An unexpected error occurred. Please try again later." }
        });
    }
}



        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("User store does not support email.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
