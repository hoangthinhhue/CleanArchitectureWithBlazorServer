// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using Mgr.Core.Extensions;
using Mgr.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Mgr.Core.Service;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(
        IHttpContextAccessor httpContextAccessor
       )
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string UserId => _httpContextAccessor.HttpContext?.User.GetUserId()??string.Empty;
    public string Email => _httpContextAccessor.HttpContext?.User.GetEmail() ?? string.Empty;
    public string UserName => _httpContextAccessor.HttpContext?.User.GetUserName() ?? string.Empty;
    public string TenantId => _httpContextAccessor.HttpContext?.User.GetTenantId() ?? string.Empty;
    public string TenantName => _httpContextAccessor.HttpContext?.User.GetTenantName() ?? string.Empty;
    public string DisplayName => _httpContextAccessor.HttpContext?.User.GetDisplayName() ?? string.Empty;
    public string PhoneNumber => _httpContextAccessor.HttpContext?.User.GetPhoneNumber() ?? string.Empty;
    public string ProfilePictureDataUrl => _httpContextAccessor.HttpContext?.User.GetProfilePictureDataUrl() ?? string.Empty;
    public string[] AssignRoles
    {
        get
        {
            var str = _httpContextAccessor.HttpContext?.User.GetAssignRoles() ?? string.Empty;
            if (string.IsNullOrEmpty(str))
            {
                return new string[] { };
            }
            return str.Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
