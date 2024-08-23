using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.DeleteById;

public class DeleteExpenseByIdUseCase : IDeleteExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly ILoggedUser _loggedUser;

    public DeleteExpenseByIdUseCase(
        IExpensesRepository repository,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id)
    {
        var user = await _loggedUser.Get();
        
        var expense = await _repository.GetById(id, user.Id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }
        
        await _repository.DeleteById(id, user.Id);
    }
}