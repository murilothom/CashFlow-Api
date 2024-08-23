using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebApi.Test.Users.Register;

public class RegisterUserTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string BASE_URL = "api/user";
    
    private readonly HttpClient _httpClient;
    
    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
    }
    
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterUserDtoBuilder.Build();

        // Act
        var result = await _httpClient.PostAsJsonAsync(BASE_URL, request);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }    
}