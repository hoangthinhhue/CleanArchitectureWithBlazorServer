﻿using MediatR;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Mgr.Core.Entities;
using Uni.Core.Helper;
using Mgr.Core.Interface;

namespace Uni.Core.Commands;

public class GetBaseCommand<T, Tkey> : IRequest<MethodResult<T>>
   where T : IBaseEntity<Tkey>
    where Tkey : struct
{
    public Tkey Id { get; set; }
}

public class GetBaseCommandHandler<TDbContext, T, Tkey>
   : BaseCommand<TDbContext, T, Tkey>, IRequestHandler<GetBaseCommand<T, Tkey>, MethodResult<T>>
   where TDbContext : DbContext
   where T : IBaseEntity<Tkey>
   where Tkey : struct
{
    private readonly IBaseRepository<TDbContext, T> _repository;

    public GetBaseCommandHandler()
    {
        _repository = HttpContextInfo.GetRequestService<IBaseRepository<TDbContext, T>>();
    }

    public async Task<MethodResult<T>> Handle(GetBaseCommand<T, Tkey> request, CancellationToken cancellationToken)
    {
        var model = await _repository.FindAsync(q => q.Id.Equals(request.Id));
        if (model == null)
            return MethodResult<T>.ResultWithError(status: (int)HttpStatusCode.NotFound, "No have content");
        return MethodResult<T>.ResultWithData(model);
    }
}