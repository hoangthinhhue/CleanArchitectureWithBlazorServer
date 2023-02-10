// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.AuditTrails.Caching;
using CleanArchitecture.Blazor.Application.Features.AuditTrails.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.AuditTrails.Queries.PaginationQuery;

public class AuditTrailsWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<AuditTrailDto>>
{
    public string CacheKey => AuditTrailsCacheKey.GetPaginationCacheKey($"{this}");
    public MemoryCacheEntryOptions? Options => AuditTrailsCacheKey.MemoryCacheEntryOptions;
}
public class AuditTrailsQueryHandler : IRequestHandler<AuditTrailsWithPaginationQuery, PaginatedData<AuditTrailDto>>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public AuditTrailsQueryHandler(
        ICurrentUserService currentUserService,
        ApplicationDbContext context,
        IMapper mapper
        )
    {
        _currentUserService = currentUserService;
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedData<AuditTrailDto>> Handle(AuditTrailsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.AuditTrails
            .Where(x=>x.TableName!.Contains(request.Keyword))
            .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<AuditTrailDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);

        return data;
    }


}
