// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.AuditTrails.Caching;
using CleanArchitecture.Blazor.Application.Features.Loggers.Caching;
using CleanArchitecture.Blazor.Domain.DTOs.Loggers.DTOs;
using CleanArchitecture.Blazor.Domain.Interfaces;
using Mgr.Core.Extensions;
using Mgr.Core.Interface;
using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Application.Logs.Queries.PaginationQuery;
public class LogsWithPaginationQuery : PaginationRequest, ICacheableRequest<PaginatedData<LogDto>>
{
    public string CacheKey => LogsCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => LogsCacheKey.MemoryCacheEntryOptions;
}
public class LogsQueryHandler : IRequestHandler<LogsWithPaginationQuery, PaginatedData<LogDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public LogsQueryHandler(
        ICurrentUserService currentUserService,
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _currentUserService = currentUserService;
        _context = context;
        _mapper = mapper;
    }
    public async Task<PaginatedData<LogDto>> Handle(LogsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Loggers
                .ProjectTo<LogDto>(_mapper.ConfigurationProvider)
                .ToPageListAsync(request);
        return data;
    }


}
