using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

public class CashFlowDbContext : DbContext
{
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // ! FIXME: 
        var connectionString = "Server=localhost;Database=cashflowdb;Uid=root;Pwd=@Password;";
    
        var version = new Version(8, 0, 39);
        var serverVersion = new MySqlServerVersion(version);
        
        optionsBuilder.UseMySql(connectionString, serverVersion);
    }
}