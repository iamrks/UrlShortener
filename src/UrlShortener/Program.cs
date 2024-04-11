using LaunchDarkly.Sdk.Server;
using Microsoft.EntityFrameworkCore;
using UrlShortener;
using UrlShortener.Extensions;
using UrlShortener.Services;
using UrlShortener.Services.FeatureFlag;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// setup EF Core
builder.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

// Register services
builder.Services.AddScoped<UrlShorteningService>();

builder.Services.AddHttpClient<GithubService>();

builder.Services.AddSeqLogging(builder.Configuration);

var ldKey = builder.Configuration["LaunchDarkly:SdkKey"];
builder.Services.AddSingleton(new LdClient(Configuration.Default(ldKey)));
builder.Services.AddSingleton<ILdContextService, LdContextService>();
builder.Services.AddSingleton<IFeatureFlagService, LaunchDarklyFeatureFlagService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
