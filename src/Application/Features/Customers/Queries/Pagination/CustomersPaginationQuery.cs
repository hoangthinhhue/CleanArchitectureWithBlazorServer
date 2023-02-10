﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Queries.Pagination;

    public class CustomersWithPaginationQuery : PaginationFilter, ICacheableRequest<PaginatedData<CustomerDto>>
    {
        public string CacheKey => CustomerCacheKey.GetPaginationCacheKey($"{this}");
        public MemoryCacheEntryOptions? Options => CustomerCacheKey.MemoryCacheEntryOptions;
    }
    
    public class CustomersWithPaginationQueryHandler :
         IRequestHandler<CustomersWithPaginationQuery, PaginatedData<CustomerDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CustomersWithPaginationQueryHandler> _localizer;

        public CustomersWithPaginationQueryHandler(
            ApplicationDbContext context,
            IMapper mapper,
            IStringLocalizer<CustomersWithPaginationQueryHandler> localizer
            )
        {
            _context = context;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<PaginatedData<CustomerDto>> Handle(CustomersWithPaginationQuery request, CancellationToken cancellationToken)
        {
            // TODO: Implement CustomersWithPaginationQueryHandler method 
           var data = await _context.Customers//.Where(x=>x.Name!.Contains(request.Keyword) || x.Description!.Contains(request.Keyword))
                .OrderBy($"{request.OrderBy} {request.SortDirection}")
                .ProjectTo<CustomerDto>(_mapper.ConfigurationProvider)
                .PaginatedDataAsync(request.PageNumber, request.PageSize);
            return data;
        }
   }