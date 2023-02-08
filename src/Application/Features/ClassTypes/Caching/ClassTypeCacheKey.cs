// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.Caching;

public static class ClassTypeCacheKey
{
    private static readonly TimeSpan refreshInterval = TimeSpan.FromHours(1);
    public const string GetAllCacheKey = "all-ClassTypes";
    public static string GetPaginationCacheKey(string parameters)
    {
        return $"ClassTypesWithPaginationQuery,{parameters}";
    }
    static ClassTypeCacheKey()
    {
        _tokensource = new CancellationTokenSource(refreshInterval);
    }
    private static CancellationTokenSource _tokensource;
    public static CancellationTokenSource SharedExpiryTokenSource() {
        if (_tokensource.IsCancellationRequested)
        {
            _tokensource = new CancellationTokenSource(refreshInterval);
        }
        return _tokensource;
    }
    public static MemoryCacheEntryOptions MemoryCacheEntryOptions => new MemoryCacheEntryOptions().AddExpirationToken(new CancellationChangeToken(SharedExpiryTokenSource().Token));
}

