using LaunchDarkly.Sdk.Server;
using Microsoft.EntityFrameworkCore;
using Polly;
using Swashbuckle.AspNetCore.Filters;
using System.Net.Http.Headers;
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
        AddGithubHttpClient(services, configuration);
        
        services.AddScoped<UrlShorteningService>();
        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

        return services;
    }

    private static void AddGithubHttpClient(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IGithubService, GithubService>(c =>
        {
            var config = configuration.GetSection("HttpClient:Github");

            c.BaseAddress = new Uri(config?["BaseAddress"] ?? "");
            c.DefaultRequestHeaders.Add("User-Agent", "C# HttpClient");
            c.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config?["Token"]);
        })
        .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(2)))
        .AddTransientHttpErrorPolicy(policy => policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(5)));
    }

    public static IServiceCollection AddCache(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.ConfigurationOptions = new StackExchange.Redis.ConfigurationOptions()
            {
                User = configuration.GetValue<string>("Redis:User"),
                Password = configuration.GetValue<string>("Redis:Password"),
            };
            options.ConfigurationOptions.EndPoints.Add(configuration.GetValue<string>("Redis:Endpoint") ?? string.Empty);
        });

        services.AddHybridCache(options =>
        {
            //options.DefaultEntryOptions = new HybridCacheEntryOptions
            //{
            //    Expiration = TimeSpan.FromSeconds(10),
            //    LocalCacheExpiration = TimeSpan.FromSeconds(5)
            //};
        });

        return services;
    }

    public static void AddSwaggerSupport(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.ExampleFilters();
        });

        services.AddSwaggerExamplesFromAssemblies(typeof(Program).Assembly);
    }
}