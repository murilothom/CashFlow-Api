using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllExpensesUseCase(
        IExpensesRepository repository,
        IMapper mapper,
        ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseExpensesDto> Execute()
    {
        var user = await _loggedUser.Get();
        
        var result = await _repository.GetAll(user.Id);

        return new ResponseExpensesDto
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseDto>>(result),
        };
    }
}