using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gruppeportalen.Areas.PrivateUser.HelperClasses;

public class PrivateUserInformationCheckFactoryAttribute : TypeFilterAttribute
{
    public PrivateUserInformationCheckFactoryAttribute() : base(typeof(InformationExistsRedirectFilter))
    {
        
    }

    private class InformationExistsRedirectFilter : IAuthorizationFilter
    {
        private readonly UserManager<ApplicationUser> _um;
        private readonly PrivateUserOperations _pus;

        public InformationExistsRedirectFilter(UserManager<ApplicationUser> um, PrivateUserOperations pus)
        {
            _um = um;
            _pus = pus;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = await _um.GetUserAsync(context.HttpContext.User);

            if (user != null && !_pus.PrivateUserExists(user.Id))
            {
                context.Result = new RedirectToActionResult("Index", "Add", new { area = "PrivateUser" });
            }
        }
    }
}