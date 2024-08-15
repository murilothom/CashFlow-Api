using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(
        IExpensesRepository repository, 
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseRegisterExpenseDto> Execute(RegisterExpenseDto request)
    {
        Validate(request);

        var entity = _mapper.Map<Expense>(request);
        
        await _repository.Add(entity);

        var response = _mapper.Map<ResponseRegisterExpenseDto>(entity);

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