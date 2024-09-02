using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.User.GetProfile;

public interface IGetUserProfileUseCase
{
    Task<ResponseUserProfileDto> Execute();
}