using Gruppeportalen.Data;
using Gruppeportalen.Models;
using Gruppeportalen.Services.Classes;
using Gruppeportalen.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gruppeportalen.Areas.PrivateUser.HelperClasses;

public class AdminForThisGroupCheckFactoryAttribute :TypeFilterAttribute
{

    public AdminForThisGroupCheckFactoryAttribute() : base(typeof(NotAdminForGroupRedirectFilter))
    {
        
    }
    
    private class NotAdminForGroupRedirectFilter : IAuthorizationFilter
    {

        private readonly UserManager<ApplicationUser> _um;
        private readonly ApplicationDbContext _db;
        private readonly ILocalGroupAdminService _lgas;

        public NotAdminForGroupRedirectFilter(UserManager<ApplicationUser> um, ILocalGroupAdminService lgas, ApplicationDbContext db)
        {
            _um = um;
            _lgas = lgas;
            _db = db;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool valid = true;
            var user = _um.GetUserAsync(context.HttpContext.User).GetAwaiter().GetResult();
            Guid groupId;

            if (user != null && context.RouteData.Values.TryGetValue("groupId", out var groupIdValue)
                             && Guid.TryParse(groupIdValue?.ToString(), out groupId))
            {
                var allRouteData = string.Join(", ", context.RouteData.Values.Select(kv => $"{kv.Key}: {kv.Value}"));
                Console.WriteLine($"RouteData does not contain 'groupId'. Current RouteData: {allRouteData}");
                valid = _lgas.DoesAdminExist(user.Id, groupId);
            }
            else
            {
                valid = false;
            }
            
            if (valid == false)
            {
                context.Result = new RedirectToActionResult("Admin", "Oopsie", new { area = "" });;
            }
            
        }
    }
}