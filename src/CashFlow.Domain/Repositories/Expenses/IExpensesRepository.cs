using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll(Guid userId);
    Task<Expense?> GetById(long id, Guid userId);
    Task UpdateById(long id, Expense expense);
    Task DeleteById(long id, Guid userId);
    Task<List<Expense>> FilterByMonth(DateOnly date, Guid userId);
}