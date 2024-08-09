using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Expense>> GetAll()
    {
        var result = await _dbContext.Expenses
            .AsNoTracking()
            .ToListAsync();
        return result;
    }

    public async Task<Expense?> GetById(long id)
    {
        var response = await _dbContext.Expenses
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id.Equals(id));
        return response;
    }

    public async Task UpdateById(long id, Expense expense)
    {
        var dbExpense = await _dbContext.Expenses.FindAsync(id);
        if (dbExpense is not null)
        {
            _dbContext.Entry(dbExpense).CurrentValues.SetValues(expense);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task DeleteById(long id)
    {
        var dbExpense = await _dbContext.Expenses.FindAsync(id);
        if (dbExpense is not null)
        {
            _dbContext.Expenses.Remove(dbExpense);
            await _dbContext.SaveChangesAsync();
        }
    }
    
    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1);
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

        var expenses = await _dbContext.Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();

        return expenses;
    }
}