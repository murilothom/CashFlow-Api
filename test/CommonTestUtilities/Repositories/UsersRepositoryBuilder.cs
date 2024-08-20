using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommonTestUtilities.Repositories;

public class UsersRepositoryBuilder
{
    private readonly Mock<IUsersRepository> _repository;

    public UsersRepositoryBuilder()
    {
        _repository = new Mock<IUsersRepository>();
    }

    public void GetByEmail(string email)
    {
        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = email,
            Name = "Existent User",
            Password = "encrypted_password",
            Role = Role.TEAM_MEMBER
        };
        
        _repository.Setup(repo => repo.GetByEmail("existent@email.com")).ReturnsAsync(user);
    }

    public IUsersRepository Build() => _repository.Object;
}