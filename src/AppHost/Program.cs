var builder = DistributedApplication.CreateBuilder(args);

var urlShortenerApi = builder.AddProject<Projects.UrlShortener>("UrlShortenerApi");

builder.Build().Run();
