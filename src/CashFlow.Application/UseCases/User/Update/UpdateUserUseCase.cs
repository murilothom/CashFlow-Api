using CashFlow.Communication.Requests;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using FluentValidation;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUsersRepository _repository;

    public UpdateUserUseCase(ILoggedUser loggedUser, IUsersRepository repository)
    {
        _loggedUser = loggedUser;
        _repository = repository;
    }
    public async Task Execute(RequestUpdateUserDto request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);
        
        var user = await _repository.GetById(loggedUser.Id);

        user.Email = request.Email;
        user.Name = request.Name;
        
        await _repository.Update(user);
    }

    private async Task Validate(RequestUpdateUserDto request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = await validator.ValidateAsync(request);

        if (currentEmail.Equals(request.Email) == false)
        {
            var userExist = await _repository.GetByEmail(request.Email);
            if (userExist != null)
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_IN_USE));
            }
        }

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}