using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentAssertions;

namespace WebApi.Test.Login;

public class LoginTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string BASE_URL = "api/login";

    private readonly HttpClient _httpClient;

    public LoginTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = new RequestLoginDto()
        {
            Email = "test@example.com",
            Password = "!Password123",
        };

        // Act
        var result = await _httpClient.PostAsJsonAsync(BASE_URL, request);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        response.RootElement.GetProperty("name").GetString().Should().Be("Test User");
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
}