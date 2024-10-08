﻿using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.UpdateById;

public class UpdateExpenseByIdUseCase : IUpdateExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    
    public UpdateExpenseByIdUseCase(
        IExpensesRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id, RegisterExpenseDto request)
    {
        Validate(request);
        
        var user = await _loggedUser.Get();
        
        var expense = await _repository.GetById(id, user.Id);

        if (expense is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        var updatedExpenseEntity = _mapper.Map(request, expense);
        
        await _repository.UpdateById(id, updatedExpenseEntity);
    }

    private void Validate(RegisterExpenseDto request)
    {
        var validator = new ExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}