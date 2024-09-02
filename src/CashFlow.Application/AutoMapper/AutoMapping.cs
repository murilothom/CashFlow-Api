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
        CreateMap<RequestRegisterUserDto, User>()
            .ForMember(
                user => user.Password,
                config => config.Ignore());

        CreateMap<RegisterExpenseDto, Expense>()
            .ForMember(
                dest => dest.Tags,
                config => config.MapFrom(source => source.Tags.Distinct()));

        CreateMap<Communication.Enums.Tag, Tag>()
            .ForMember(
                dest => dest.Value,
                config => config.MapFrom(source => source));
    }
    
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpenseDto>();
        CreateMap<Expense, ResponseShortExpenseDto>();
        CreateMap<Expense, ResponseExpenseDto>();
        CreateMap<User, ResponseUserProfileDto>();
    }
}