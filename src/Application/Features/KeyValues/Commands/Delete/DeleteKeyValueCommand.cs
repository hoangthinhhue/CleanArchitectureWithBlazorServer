// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.KeyValues.Caching;
using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Application.Features.KeyValues.Commands.Delete;

public class DeleteKeyValueCommand : ICacheInvalidatorRequest<MethodResult>
{
    public int[] Id { get; }
    public string CacheKey => KeyValueCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => KeyValueCacheKey.SharedExpiryTokenSource();
    public DeleteKeyValueCommand(int[] id)
    {
        Id = id;
    }
}


public class DeleteKeyValueCommandHandler : IRequestHandler<DeleteKeyValueCommand, MethodResult>
   
{
    private readonly IApplicationDbContext _context;

    public DeleteKeyValueCommandHandler(
        IApplicationDbContext context
        )
    {
        _context = context;
    }
    public async Task<MethodResult> Handle(DeleteKeyValueCommand request, CancellationToken cancellationToken)
    {
        var items = await _context.KeyValues.Where(x => request.Id.Contains(x.Id)).ToListAsync(cancellationToken);
        foreach (var item in items)
        {
            var changeEvent = new UpdatedEvent<KeyValue>(item);
            item.AddDomainEvent(changeEvent);
            _context.KeyValues.Remove(item);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return await MethodResult.SuccessAsync();
    }
}
