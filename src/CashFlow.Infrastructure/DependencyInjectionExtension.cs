using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Extensions;
using CashFlow.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.IsTesting() == false)
        {
            AddDbContext(services, configuration);
        }
        AddToken(services, configuration);
        AddRepositories(services);

        services.AddScoped<IPasswordEncrypter, Security.Cryptography.PasswordEncrypter>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeInMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresInMinutes");
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigninKey");

        if (string.IsNullOrEmpty(signinKey))
        {
            throw new InvalidOperationException("Invalid signinKey.");
        }

        services.AddScoped<IAccessTokenGenerator>(serviceProvider => new JwtTokenGenerator(expirationTimeInMinutes, signinKey));
    }

    private static void AddRepositories(IServiceCollection services)
    {
       services.AddScoped<IExpensesRepository, ExpensesRepository>();
       services.AddScoped<IUsersRepository, UsersRepository>();
    }
    
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
    
        var serverVersion = ServerVersion.AutoDetect(connectionString);

        services.AddDbContext<CashFlowDbContext>(dbContext => dbContext.UseMySql(connectionString, serverVersion));
    }
}