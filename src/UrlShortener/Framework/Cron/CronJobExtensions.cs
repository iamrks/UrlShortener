using Hangfire;
using UrlShortener.Extensions;

namespace UrlShortener.Framework.Cron;

public static class CronJobExtensions
{
    public static IServiceCollection AddCronJobServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.Enabled("CronJobs")) return services;

        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetConnectionString("Database"));
        });

        services.AddHangfireServer(options => configuration.GetSection("CronJobs").Bind(options));
        services.AddTransient<CronJobManager>();

        return services;
    }

    public static IApplicationBuilder RegisterCronJobs(this IApplicationBuilder app, IConfiguration configuration)
    {
        var services = app.ApplicationServices;
        if (!configuration.Enabled("CronJobs")) return app;
        var cronJobManager = services.GetRequiredService<CronJobManager>();
        cronJobManager.RegisterJobs();
        return app;
    }
}