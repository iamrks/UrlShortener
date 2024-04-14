namespace UrlShortener.Tests.Controllers;

public class PingControllerTest(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory = factory;

    [Fact]
    public async Task Get_ReturnCorrectEnvironment()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/ping");
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal("Staging", responseBody);
    }
}
