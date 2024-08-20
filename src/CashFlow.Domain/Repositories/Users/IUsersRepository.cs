using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetByEmail(string email);
}