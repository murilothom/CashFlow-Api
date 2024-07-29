﻿using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository _repository;

    public RegisterExpenseUseCase(IExpensesRepository repository)
    {
        _repository = repository;
    }

    public ResponseRegisterExpenseDto Execute(RequestRegisterExpenseDto request)
    {
        Validate(request);

        var entity = new Expense
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            Title = request.Title,
            PaymentType = (PaymentType)request.PaymentType
        };
        
        _repository.Add(entity);
        
        return new ResponseRegisterExpenseDto();
    }

    private void Validate(RequestRegisterExpenseDto request)
    {
        var validator = new RegisterExpenseValidator();

        var result = validator.Validate(request);

        if (result.IsValid)
        {
            return;
        }
        var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

        throw new ErrorOnValidationException(errorMessages);
    }
}