using HealthyTips.Application.Database;
using HealthyTips.Application.Events.Listeners;
using HealthyTips.Application.Models;
using HealthyTips.Application.Services.Auth;
using HealthyTips.Application.Services;
using Spark.Library.Database;
using Spark.Library.Logging;
using Coravel;
using Microsoft.AspNetCore.Components.Authorization;
using Spark.Library.Auth;
using HealthyTips.Application.Jobs;
using Spark.Library.Mail;

namespace HealthyTips.Application.Startup;

public static class AppServiceRegistration
{
    public static IServiceCollection AddAppServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddCustomServices();
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddDatabase<DatabaseContext>(config);
        services.AddAutoMapper(typeof(Program).Assembly);
        services.AddLogger(config);
        services.AddAuthorization(config, new string[] { CustomRoles.Admin, CustomRoles.User });
        services.AddAuthentication<IAuthValidator>(config);
        services.AddScoped<AuthenticationStateProvider, SparkAuthenticationStateProvider>();
        services.AddJobServices();
        services.AddScheduler();
        services.AddQueue();
        services.AddEventServices();
        services.AddEvents();
        services.AddMailer(config);
        return services;
    }

    private static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        // add custom services
        services.AddScoped<UsersService>();
        services.AddScoped<RolesService>();
			services.AddScoped<IAuthValidator, AuthValidator>();
			services.AddScoped<AuthService>();
			return services;
    }

    private static IServiceCollection AddEventServices(this IServiceCollection services)
    {
        // add custom events here
        services.AddTransient<EmailNewUser>();
        return services;
    }

    private static IServiceCollection AddJobServices(this IServiceCollection services)
    {
        // add custom background tasks here
        services.AddTransient<ExampleJob>();
        return services;
    }
}
