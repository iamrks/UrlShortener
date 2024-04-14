using UrlShortener.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Custom services
builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.AddSeqLogging(builder.Configuration);
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddLaunchDarkly(builder.Configuration);

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
