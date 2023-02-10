// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Blazor.Application.Features.Loggers.Caching;
using CleanArchitecture.Blazor.Application.Features.Loggers.DTOs;

namespace CleanArchitecture.Blazor.Application.Features.Loggers.Queries.ChatData;

public class LogsTimeLineChatDataQuery : ICacheableRequest<List<LogTimeLineDto>>
{
    public DateTime LastDateTime { get; set; } = DateTime.Now.AddDays(-60);
    public string CacheKey => LogsCacheKey.GetChartDataCacheKey(LastDateTime.ToString());
    public MemoryCacheEntryOptions? Options => LogsCacheKey.MemoryCacheEntryOptions;
}

public class LogsChatDataQueryHandler :
     IRequestHandler<LogsTimeLineChatDataQuery, List<LogTimeLineDto>>

{
    private readonly ApplicationDbContext _context;
    private readonly IStringLocalizer<LogsChatDataQueryHandler> _localizer;

    public LogsChatDataQueryHandler(
        ApplicationDbContext context,
        IStringLocalizer<LogsChatDataQueryHandler> localizer
        )
    {
        _context = context;
        _localizer = localizer;
    }
    public async Task<List<LogTimeLineDto>> Handle(LogsTimeLineChatDataQuery request, CancellationToken cancellationToken)
    {
        var data = await _context.Loggers.Where(x => x.TimeStamp >= request.LastDateTime)
                  .GroupBy(x => new {  x.TimeStamp.Date })
                  .Select(x => new { x.Key.Date, Total = x.Count() })
                  .OrderBy(x => x.Date)
                  .ToListAsync(cancellationToken);
        
        var result = new List<LogTimeLineDto>();
        var end = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,0, 0, 0);
        var start = request.LastDateTime.Date;

        while (start <= end)
        {
                var item = data.Where(x => x.Date == start.Date).FirstOrDefault();
                if (item != null)
                {
                    result.Add(new LogTimeLineDto { dt = item.Date, total = item.Total });
                }
                else
                {
                    result.Add(new LogTimeLineDto { dt = start, total = 0 });

                }
            
            start = start.AddDays(1);
        }
        return result.OrderBy(x=>x.dt).ToList();
    }
}
