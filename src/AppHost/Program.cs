var builder = DistributedApplication.CreateBuilder(args);

var urlShortenerApi = builder.AddProject<Projects.UrlShortener>("UrlShortenerApi");

builder.AddNpmApp("angular", "../UrlShortener.Client")
    .WithReference(urlShortenerApi)
    .WaitFor(urlShortenerApi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();


builder.Build().Run();
