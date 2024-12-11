using System.Security.Claims;
using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IUserService
{
    Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal user);
}