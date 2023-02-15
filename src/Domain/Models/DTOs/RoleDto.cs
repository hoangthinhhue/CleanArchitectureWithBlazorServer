// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanArchitecture.Blazor.Domain.DTOs.Identity.Dto;

public class RoleDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public required string Name { get; set; }
    public string? NormalizedName { get; set; }
    public string? Description { get; set; }




}
