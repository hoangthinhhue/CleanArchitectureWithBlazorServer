// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Mgr.Core.Interfaces;

public interface ICurrentUserService
{
    string UserId { get; }
    string UserName { get; }
    string DisplayName { get; }
    string Email { get; }
    string TenantId { get; }
    string TenantName { get; }
    string ProfilePictureDataUrl { get; }
    string[] AssignRoles { get; }
}
