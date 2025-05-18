using Hangfire;
using Serilog;
using UrlShortener.Extensions;
using UrlShortener.Framework.Cron;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSupport();

// Aspire Service Defaults
builder.AddServiceDefaults();

// Register Custom services
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.AddCronJobServices(builder.Configuration);

// builder.Services.AddSeqLogging(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddLaunchDarkly(builder.Configuration);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHangfireDashboard("/jobs");
app.RegisterCronJobs(builder.Configuration);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

// app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapControllers();

app.MapDefaultEndpoints();

app.MapFallbackToFile("/index.html");

app.Run();

public partial class Program { }
