using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UsersRepository : IUsersRepository
{
    private readonly CashFlowDbContext _dbContext;
    
    public UsersRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task Add(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> getByEmail(string email)
    {
        var user = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email));

        return user;
    }
}