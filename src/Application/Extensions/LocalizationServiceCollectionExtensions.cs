using CleanArchitecture.Blazor.Infrastructure.Middlewares;
using CleanArchitecture.Core.Constants;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Blazor.Application.Extensions;
public static class LocalizationServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        => services.AddScoped<LocalizationCookiesMiddleware>()
                   .Configure<RequestLocalizationOptions>(options =>
                   {
                       options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
                       options.AddSupportedCultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
                       options.FallBackToParentUICultures = true;

                   })
                  .AddLocalization(options => options.ResourcesPath = LocalizationConstants.ResourcesPath);
}
