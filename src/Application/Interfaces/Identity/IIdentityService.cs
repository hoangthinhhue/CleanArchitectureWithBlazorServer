// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity.DTOs;
using CleanArchitecture.Blazor.Application.Features.Identity.Dto;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;

public interface IIdentityService : IService
{
    Task<MethodResult<TokenResponse>> LoginAsync(TokenRequest request, CancellationToken cancellation = default);
    Task<MethodResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellation = default);
    Task<string?> GetUserNameAsync(string userId, CancellationToken cancellation = default);
    
    Task<bool> IsInRoleAsync(string userId, string role, CancellationToken cancellation = default);
    Task<bool> AuthorizeAsync(string userId, string policyName, CancellationToken cancellation = default);
    Task<MethodResult> DeleteUserAsync(string userId, CancellationToken cancellation = default);
    Task<IDictionary<string, string?>> FetchUsers(string roleName, CancellationToken cancellation = default);
    Task UpdateLiveStatus(string userId, bool isLive, CancellationToken cancellation = default);
    Task<UserDto> GetUserDto(string userId,CancellationToken cancellation=default);
    string GetUserName(string userId);
    Task<List<UserDto>?> GetUsers(string tenantId, CancellationToken cancellation = default);
}
