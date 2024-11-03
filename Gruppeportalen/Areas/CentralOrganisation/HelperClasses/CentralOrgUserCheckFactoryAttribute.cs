using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gruppeportalen.Areas.CentralOrganisation.HelperClasses;

public class CentralOrgUserCheckFactoryAttribute : TypeFilterAttribute
{
    public CentralOrgUserCheckFactoryAttribute() : base(typeof(WrongUserRedirectFilter))
    {
    }

    private class WrongUserRedirectFilter : IAuthorizationFilter
    {
        private readonly UserManager<ApplicationUser> _um;

        public WrongUserRedirectFilter(UserManager<ApplicationUser> um)
        {
            _um = um;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = await _um.GetUserAsync(context.HttpContext.User);

            if (user != null && user.TypeOfUser != Constants.Centralorg)
            {
                context.Result = new RedirectToActionResult("Index", "Oops", new {area = ""});
            }
        }
    }
}