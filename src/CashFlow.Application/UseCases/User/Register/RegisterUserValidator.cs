using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RequestRegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.EMPTY_NAME);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMPTY_EMAIL)
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.INVALID_EMAIL);

        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserDto>());
    }
}