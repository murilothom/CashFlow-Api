﻿using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesRepository
{
    Task Add(Expense expense);
    Task<List<Expense>> GetAll();
    Task<Expense?> GetById(long id);
    Task UpdateById(long id, Expense expense);
    Task DeleteById(long id);
    Task<List<Expense>> FilterByMonth(DateOnly date);
}