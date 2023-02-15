using System.Reflection;
using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using Mgr.Core.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.Blazor.Infrastructure.Extensions;
public static class AuthenticationServiceCollectionExtensions
{
    public static IServiceCollection AddAuthenticationService(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddTransient<ITicketStore, InMemoryTicketStore>();
        services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureCookieAuthenticationOptions>();
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = false;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // User settings
            options.User.RequireUniqueEmail = true;

        });
        services
                 .AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsIdentityFactory>()
                 .AddTransient<IIdentityService, IIdentityService>()
                 .AddAuthorization(options =>
                 {
                     options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
                     // Here I stored necessary permissions/roles in a constant
                     foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                     {
                         var propertyValue = prop.GetValue(null);
                         if (propertyValue is not null)
                         {
                             options.AddPolicy((string)propertyValue, policy => policy.RequireClaim(ApplicationClaimTypes.Permission, (string)propertyValue));
                         }
                     }
                 })
                 .AddAuthentication()
                 .AddCookie(options =>
                 {
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                     options.SlidingExpiration = true;
                     options.AccessDeniedPath = "/";
                 });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.Configure<CookiePolicyOptions>(options =>
                 {
                     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                     options.CheckConsentNeeded = context => true;
                     options.MinimumSameSitePolicy = SameSiteMode.None;

                 });
        services.ConfigureApplicationCookie(options =>
         {
             options.Cookie.HttpOnly = true;
             options.Events.OnRedirectToLogin = context =>
             {
                 context.Response.StatusCode = 401;
                 return Task.CompletedTask;
             };
         });
        return services;
    }

}
