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

namespace Gruppeportalen.Areas.Identity.Pages.Account
{
    public class RegisterPartialModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterPartialModel> _logger;

        public RegisterPartialModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<RegisterPartialModel> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
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
            if (!ModelState.IsValid)
            {
                return new JsonResult(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var user = CreateUser();
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            user.TypeOfUser = Input.TypeOfUser;

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                var userId = user.Id;
                return new JsonResult(new { success = true, userId });
            }

            return new JsonResult(new
            {
                success = false,
                errors = result.Errors.Select(e => e.Description).ToList()
            });
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
