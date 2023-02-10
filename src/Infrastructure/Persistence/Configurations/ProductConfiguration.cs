// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using Core.Common;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence.Configurations;
#nullable disable
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.Ignore(e => e.DomainEvents);
        builder.Property(e => e.Pictures)
           .HasConversion(
                 v => JsonSerializer.Serialize(v, DefaultJsonSerializerOptions.Options),
                 v => JsonSerializer.Deserialize<IList<string>>(v, DefaultJsonSerializerOptions.Options),
                 new ValueComparer<IList<string>>(
                        (c1, c2) => c1.SequenceEqual(c2),
                               c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                               c => c.ToList()));

        
    }
}
