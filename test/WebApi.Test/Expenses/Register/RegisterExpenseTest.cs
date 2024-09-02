using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace WebApi.Test.Expenses.Register;

public class RegisterExpenseTest : IClassFixture<CustomWebApplicationFactory>
{
    private const string BASE_URL = "api/expenses";

    private readonly HttpClient _httpClient;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory)
    {
        _httpClient = webApplicationFactory.CreateClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", webApplicationFactory.GetToken());
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterExpenseDtoBuilder.Build();

        // Act
        var result = await _httpClient.PostAsJsonAsync(BASE_URL, request);
        
        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        
        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }
}