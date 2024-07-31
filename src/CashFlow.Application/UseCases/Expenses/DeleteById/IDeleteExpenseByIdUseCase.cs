namespace CashFlow.Application.UseCases.Expenses;

public interface IDeleteExpenseByIdUseCase
{
    Task Execute(long id);
}