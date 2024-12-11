using Gruppeportalen.Services.Interfaces;
using Gruppeportalen.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Gruppeportalen.Services.Classes;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _um;

    public UserService(UserManager<ApplicationUser> um)
    {
        _um = um;
    }
    
    public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        return await _um.GetUserAsync(user);
    }
}