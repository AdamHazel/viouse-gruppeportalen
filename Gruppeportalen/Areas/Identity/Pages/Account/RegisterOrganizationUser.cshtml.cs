using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Gruppeportalen.Data;
using Gruppeportalen.Models;
using OrganizationUserModel = Gruppeportalen.Areas.CentralOrganisation.Models.CentralOrganisation;

namespace Gruppeportalen.Areas.Identity.Pages.Account
{
    public class RegisterOrganizationUserModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<RegisterOrganizationUserModel> _logger;

        public RegisterOrganizationUserModel(ApplicationDbContext db, ILogger<RegisterOrganizationUserModel> logger)
        {
            _db = db;
            _logger = logger;
        }

        [BindProperty]
        public OrganizationUserInputModel Input { get; set; } = new();

        public class OrganizationUserInputModel
        {
            public string UserId { get; set; } // ID from ApplicationUser

            // Organization user fields
            public string OrganisationNumber { get; set; }
            public string OrganisationName { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray();
                return new JsonResult(new { success = false, errors });
            }

            // Add the new organization user record to the database
            var organizationUser = new OrganizationUserModel
            {
                Id = Input.UserId,
                OrganisationNumber = Input.OrganisationNumber,
                OrganisationName = Input.OrganisationName
            };

            _db.CentralOrganisations.Add(organizationUser);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Organization user registered with ID: {UserId}", Input.UserId);

            return new JsonResult(new { success = true });
        }
    }
}
