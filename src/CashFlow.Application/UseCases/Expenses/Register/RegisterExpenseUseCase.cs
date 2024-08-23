using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    
    public RegisterExpenseUseCase(
        IExpensesRepository repository, 
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseRegisterExpenseDto> Execute(RegisterExpenseDto request)
    {
        Validate(request);
        
        var user = await _loggedUser.Get();
        
        var expense = _mapper.Map<Expense>(request);
        expense.UserId = user.Id;
        
        await _repository.Add(expense);

        var response = _mapper.Map<ResponseRegisterExpenseDto>(expense);

        return response;
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