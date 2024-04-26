﻿using ActualLab.Fusion;
using ActualLab.Fusion.EntityFramework;
using ActualLab.Fusion.Extensions;

namespace CleanArchitecture.Blazor.Server.UI.Services.Fusion;

public class OnlineUserTracker(IKeyValueStore store) : IOnlineUserTracker
{
    private DbShard _shard = DbShard.None;
    public async Task AddUser(string userId, CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
            _ = GetOnlineUsers();
        if(!string.IsNullOrEmpty(userId))
        await store.Set(_shard, $"U/{userId}", DateTime.UtcNow.ToString());

    }

    public Task<string[]> GetOnlineUsers(CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
            return default!;
        
        return store.ListKeySuffixes(_shard,"U", PageRef.New<string>(int.MaxValue));
    }

    public async Task RemoveUser(string userId, CancellationToken cancellationToken = default)
    {
        if (Computed.IsInvalidating())
            _ = GetOnlineUsers();

        await store.Remove(_shard, $"U/{userId}");
    }
}
