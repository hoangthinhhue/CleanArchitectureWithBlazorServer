
using CleanArchitecture.Blazor.Infrastructure.Middlewares;
using CleanArchitecture.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Blazor.Application.Extensions;
public static class ServicesCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddScoped<ExceptionHandlingMiddleware>()
                   .AddScoped<ICurrentUserService, CurrentUserService>()
                   .AddScoped<IDateTime, DateTimeService>()
                   .AddScoped<IExcelService, ExcelService>()
                   .AddScoped<IPDFService, PDFService>()
                   .AddScoped<IUploadService, UploadService>();
}
