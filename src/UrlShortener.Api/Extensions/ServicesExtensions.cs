namespace UrlShortener.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddSeqLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(loginBuilder =>
        {
            loginBuilder.ClearProviders();
            loginBuilder.AddConsole();
            loginBuilder.AddSeq(configuration.GetValue<string>("Logging:Seq:ServerUrl"), configuration.GetValue<string>("Logging:Seq:ApiKey"));
        });

        return services;
    }
}