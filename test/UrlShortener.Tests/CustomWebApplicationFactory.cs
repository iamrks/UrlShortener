using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data.Common;
using System.Text;

namespace UrlShortener.Tests
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Staging");

            builder.ConfigureAppConfiguration((ctx, config) =>
            {
                var keyValuePairs = Enumerable.Empty<KeyValuePair<string, string?>>()
                    .Append(new KeyValuePair<string, string?>("HttpClient:Github:Token", ""))
                    .Append(new KeyValuePair<string, string?>("Logging:Seq:ServerUrl", ""))
                    .Append(new KeyValuePair<string, string?>("Logging:Seq:ApiKey", ""));

                config.AddInMemoryCollection(keyValuePairs);
            });


            builder.ConfigureServices(services =>
            {
                var dbContextDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (dbContextDescriptor is not null) services.Remove(dbContextDescriptor);

                var dbConnectionDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbConnection));

                if (dbConnectionDescriptor is not null) services.Remove(dbConnectionDescriptor);

                UseInMemoryDatabase<ApplicationDbContext>(services);
            });
        }

        private void UseInMemoryDatabase<TContext>(IServiceCollection services) where TContext : DbContext
        {
            services.AddDbContext<TContext>(options => { options.UseInMemoryDatabase("InMemoryDbForTesting"); });
        }
    }
}
