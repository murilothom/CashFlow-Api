using CashFlow.Communication.Requests;

namespace CashFlow.Application.UseCases.User.UpdatePassword;

public interface IChangePasswordUseCase
{
    Task Execute(RequestChangePasswordDto request);
}