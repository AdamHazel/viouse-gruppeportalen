using Gruppeportalen.HelperClasses;
using Gruppeportalen.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gruppeportalen.Areas.PrivateUser.HelperClasses;

public class PrivateUserCheckFactoryAttribute : TypeFilterAttribute
{

    public PrivateUserCheckFactoryAttribute() : base(typeof(NotPrivateUserRedirectFilter))
    {
        
    }
    private class NotPrivateUserRedirectFilter : IAuthorizationFilter
    {
        private readonly UserManager<ApplicationUser> _um;

        public NotPrivateUserRedirectFilter(UserManager<ApplicationUser> um)
        {
            _um = um;
        }

        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = await _um.GetUserAsync(context.HttpContext.User);

            if (user != null && user.TypeOfUser != Constants.Privateuser)
            {
                context.Result = new RedirectToActionResult("Index", "Oopsie", new { area = "" });
            }
        }
    }
}