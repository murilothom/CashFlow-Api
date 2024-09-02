using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.User.Delete;

public class DeleteCurrentUser : IDeleteCurrentUser
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUsersRepository _repository;

    public DeleteCurrentUser(ILoggedUser loggedUser, IUsersRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }

    public async Task Execute()
    {
        var loggedUser = await _loggedUser.Get();

        await _repository.Delete(loggedUser);
    }
}