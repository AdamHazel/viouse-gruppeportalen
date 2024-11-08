using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Gruppeportalen.Models;

namespace Gruppeportalen.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public bool IsEmailConfirmed { get; set; } = false;

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new JsonResult(new { success = false, message = "Invalid confirmation request." });
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new JsonResult(new { success = false, message = "Unable to load user." });
            }

            var decodedCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, decodedCode);

            if (result.Succeeded)
            {
                IsEmailConfirmed = true;
                return new JsonResult(new { success = true, message = "Thank you for confirming your email!" });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Email confirmation failed." });
            }
        }
    }
}
