﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Customers.DTOs;
using CleanArchitecture.Blazor.Application.Features.Customers.Caching;

namespace CleanArchitecture.Blazor.Application.Features.Customers.Commands.Create;

    public class CreateCustomerCommand: IMapFrom<CustomerDto>, ICacheInvalidatorRequest<MethodResult<int>>
    {
          [Description("Id")]
    public int Id {get;set;} 
    [Description("Name")]
    public string Name {get;set;} = String.Empty; 
    [Description("Description")]
    public string? Description {get;set;} 

      public string CacheKey => CustomerCacheKey.GetAllCacheKey;
      public CancellationTokenSource? SharedExpiryTokenSource => CustomerCacheKey.SharedExpiryTokenSource();
    }
    
    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, MethodResult<int>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CreateCustomerCommand> _localizer;
        public CreateCustomerCommandHandler(
            ApplicationDbContext context,
            IStringLocalizer<CreateCustomerCommand> localizer,
            IMapper mapper
            )
        {
            _context = context;
            _localizer = localizer;
            _mapper = mapper;
        }
        public async Task<MethodResult<int>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
           // TODO: Implement CreateCustomerCommandHandler method 
           var dto = _mapper.Map<CustomerDto>(request);
           var item = _mapper.Map<Customer>(dto);
           // raise a create domain event
	       item.AddDomainEvent(new CreatedEvent<Customer>(item));
           _context.Customers.Add(item);
           await _context.SaveChangesAsync(cancellationToken);
           return  await MethodResult<int>.SuccessAsync(item.Id);
        }
    }

