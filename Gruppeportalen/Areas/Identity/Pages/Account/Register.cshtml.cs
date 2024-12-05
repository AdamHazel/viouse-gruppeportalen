using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            ILogger<RegisterModel> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required] public string Email { get; set; }
            [Required] public string TypeOfUser { get; set; } // "PrivateUser" or "OrganizationUser"
            public string UserId { get; set; } 
            [Required] public string Password { get; set; }
            [Required] [Compare("Password")] public string ConfirmPassword { get; set; }

            // PrivateUser-specific fields
            [Required]
            [StringLength(30)]
            public string Firstname { get; set; } = string.Empty;
            [Required] 
            [StringLength(30)] 
            public string Lastname { get; set; } = string.Empty;
            
            [DataType(DataType.Date)]
            public DateTime DateOfBirth { get; set; }
            
            [Required]
            [StringLength(30)]
            public string Telephone { get; set; } = string.Empty;
            
            [Required]
            [StringLength(50)]
            public string Address { get; set; } = string.Empty;
            
            [Required]
            [StringLength(30)]
            public string City { get; set; } = string.Empty;
            
            [Required]
            [MinLength(4)]
            [StringLength(4)]
            public string Postcode { get; set; } = string.Empty;

            // OrganizationUser-specific fields
            [Required]
            [MaxLength(100)]
            public string OrganisationNumber { get; set; } = string.Empty;
            public string OrganisationName { get; set; } = string.Empty;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToArray();
                return new JsonResult(new { success = false, errors });
            }

            var user = CreateUser();
            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            user.TypeOfUser = Input.TypeOfUser;

            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User created with ID: {UserId}", user.Id);
                return new JsonResult(new { success = true, userId = user.Id });
            }

            return new JsonResult(new
            {
                success = false,
                errors = result.Errors.Select(e => e.Description).ToList()
            });
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