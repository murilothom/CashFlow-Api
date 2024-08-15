using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }
    
    private void RequestToEntity()
    {
        CreateMap<RegisterExpenseDto, Expense>();
        CreateMap<RequestRegisterUserDto, User>()
            .ForMember(
                user => user.Password,
                opt => opt.Ignore());
    }
    
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpenseDto>();
        CreateMap<Expense, ResponseShortExpenseDto>();
        CreateMap<Expense, ResponseExpenseDto>();
    }
}