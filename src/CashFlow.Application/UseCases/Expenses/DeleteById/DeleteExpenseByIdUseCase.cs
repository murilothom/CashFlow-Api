using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.DeleteById;

public class DeleteExpenseByIdUseCase : IDeleteExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;

    public DeleteExpenseByIdUseCase(IExpensesRepository repository)
    {
        _repository = repository;
    }
    public async Task Execute(long id)
    {
        var expense = await _repository.GetById(id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }
        
        await _repository.DeleteById(id);
    }
}