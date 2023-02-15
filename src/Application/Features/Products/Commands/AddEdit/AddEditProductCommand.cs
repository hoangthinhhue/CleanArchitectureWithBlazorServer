// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using CleanArchitecture.Blazor.Application.Features.Documents.DTOs;
using CleanArchitecture.Blazor.Application.Features.Products.Caching;
using CleanArchitecture.Blazor.Application.Features.Products.DTOs;
using Mgr.Core.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace CleanArchitecture.Blazor.Application.Features.Products.Commands.AddEdit;

public class AddEditProductCommand : IMapFrom<ProductDto>, ICacheInvalidatorRequest<MethodResult<int>>
{

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Unit { get; set; }
    public string? Brand { get; set; }
    public decimal Price { get; set; }
    public IList<string>? Pictures { get; set; }

    public IReadOnlyList<IBrowserFile>? UploadPictures { get; set; }
    public string CacheKey => ProductCacheKey.GetAllCacheKey;
    public CancellationTokenSource? SharedExpiryTokenSource => ProductCacheKey.SharedExpiryTokenSource();
}

public class AddEditProductCommandHandler : IRequestHandler<AddEditProductCommand, MethodResult<int>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    public AddEditProductCommandHandler(
        IApplicationDbContext context,
        IMapper mapper
        )
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<MethodResult<int>> Handle(AddEditProductCommand request, CancellationToken cancellationToken)
    {
        var dto = _mapper.Map<ProductDto>(request);
        if (request.Id > 0)
        {
            var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken) ?? throw new NotFoundException($"Product {request.Id} Not Found.");
            item = _mapper.Map(dto, item);
            item.AddDomainEvent(new UpdatedEvent<Product>(item));
            await _context.SaveChangesAsync(cancellationToken);
            return MethodResult<int>.ResultWithData(item.Id);
        }
        else
        {
            var item = _mapper.Map<Product>(dto);
            item.AddDomainEvent(new CreatedEvent<Product>(item));
            _context.Products.Add(item);
            await _context.SaveChangesAsync(cancellationToken);
            return MethodResult<int>.ResultWithData(item.Id);
        }

    }
}

