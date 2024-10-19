using LaunchDarkly.Sdk.Server;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Persistence.DbContexts;
using UrlShortener.Persistence.Interceptors;
using UrlShortener.Services;
using UrlShortener.Services.FeatureFlag;

namespace UrlShortener.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddSeqLogging(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loginBuilder =>
        {
            loginBuilder.ClearProviders();
            loginBuilder.AddConsole();
            loginBuilder.AddSeq(configuration.GetValue<string>("Logging:Seq:ServerUrl"), configuration.GetValue<string>("Logging:Seq:ApiKey"));
        });

        return services;
    }

    public static IServiceCollection AddLaunchDarkly(
        this IServiceCollection services, IConfiguration configuration)
    {
        var ldKey = configuration["LaunchDarkly:SdkKey"];
        services.AddSingleton(new LdClient(Configuration.Default(ldKey)));
        services.AddSingleton<ILdContextService, LdContextService>();
        services.AddSingleton<IFeatureFlagService, LaunchDarklyFeatureFlagService>();

        return services;
    }

    public static IServiceCollection AddDatabaseContext(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(
        (sp, optionsBuilder) =>
        {
            var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>();

            optionsBuilder
                .UseSqlServer(configuration.GetConnectionString("Database"));

            if (auditableInterceptor != null)
            {
                optionsBuilder.AddInterceptors(auditableInterceptor);
            }
        });

        return services;
    }

    public static IServiceCollection AddCustomServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<UrlShorteningService>();
        services.AddHttpClient<GithubService>();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        return services;
    }

}