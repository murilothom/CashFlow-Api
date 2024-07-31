using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.Expenses.UpdateById;

public interface IUpdateExpenseByIdUseCase
{
    Task Execute(long id, RegisterExpenseDto request);
}