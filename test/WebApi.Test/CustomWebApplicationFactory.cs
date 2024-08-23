using CashFlow.Domain.Entities;
using CashFlow.Infrastructure.DataAccess;
using CashFlow.Infrastructure.Security.Cryptography;
using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                
                services.AddDbContext<CashFlowDbContext>(dbContext =>
                {
                    dbContext.UseInMemoryDatabase("InMemoryDbForTesting");
                    dbContext.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                
                StartDatabase(dbContext);
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext)
    {
        var user = new User()
        {
            Name = "Test User",
            Email = "test@example.com",
            Password = new PasswordEncrypter().Encrypt("!Password123"),
        };
        
        dbContext.Users.Add(user);

        dbContext.SaveChanges();
    }
}