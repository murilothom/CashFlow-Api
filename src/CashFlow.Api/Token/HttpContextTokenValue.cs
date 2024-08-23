using CashFlow.Domain.Security.Tokens;

namespace CashFlow.Api.Token;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public HttpContextTokenValue(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string TokenOnRequest()
    {
        var authorization = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        if (authorization.StartsWith("Bearer "))
        {
            return authorization["Bearer ".Length..].Trim();
            // return authorization.Substring("Bearer ".Length).Trim();
        }
        return string.Empty;
    }
}