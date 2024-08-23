using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetExpenseByIdUseCase(
        IExpensesRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseExpenseDto> Execute(long id)
    {
        var user = await _loggedUser.Get();
        
        var response = await _repository.GetById(id, user.Id);

        if (response is null)
        {
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }

        return _mapper.Map<ResponseExpenseDto>(response);
    }
}