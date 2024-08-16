﻿using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.DataAccess.Repositories;
using CashFlow.Infrastructure.Security.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddToken(services, configuration);
        AddRepositories(services);
        
        services.AddScoped<IPasswordEncripter, Security.Cryptography.PasswordEncripter>();
    }

    private static void AddToken(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeInMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpiresInMinutes");
        var signInKey = configuration.GetValue<string>("Settings:Jwt:SignInKey");

        if (string.IsNullOrEmpty(signInKey))
        {
            throw new InvalidOperationException("Invalid signInKey.");
        }

        services.AddScoped<IAccessTokenGenerator>(serviceProvider => new JwtTokenGenerator(expirationTimeInMinutes, signInKey));
    }

    private static void AddRepositories(IServiceCollection services)
    {
       services.AddScoped<IExpensesRepository, ExpensesRepository>();
       services.AddScoped<IUsersRepository, UsersRepository>();
    }
    
    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection");
    
        var version = new Version(8, 0, 39);
        var serverVersion = new MySqlServerVersion(version);

        services.AddDbContext<CashFlowDbContext>(config => config.UseMySql(connectionString, serverVersion));
    }
}