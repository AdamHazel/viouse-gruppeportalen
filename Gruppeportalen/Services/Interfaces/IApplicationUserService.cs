﻿using Gruppeportalen.Models;

namespace Gruppeportalen.Services.Interfaces;

public interface IApplicationUserService
{
    ApplicationUser? GetPrivateUserByEmail(string emailAddress);
    ApplicationUser? GetPrivateUserById(string userId);
    bool DoesUserExist(string emailAddress);
    bool IsUserPrivateUser(string emailAddress);
}