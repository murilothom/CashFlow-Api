using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;

public interface IUsersRepository
{
    Task Add(User user);
    Task<User?> GetByEmail(string email);
    Task<User> GetById(Guid id);
    Task Delete(User user);
    Task Update(User user);
}