using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    
    public RegisterUserUseCase(
        IUsersRepository repository,
        IMapper mapper,
        IPasswordEncripter passwordEncripter)
    {
        _repository = repository;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
    }
    public async Task<ResponseRegisterUserDto> Execute(RequestRegisterUserDto request)
    {
        await Validate(request);
        
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _repository.Add(user);

        return new ResponseRegisterUserDto
        {
            Name = user.Name,
        };
    }

    private async Task Validate(RequestRegisterUserDto request)
    {
        var validator = new RegisterUserValidator();

        var result = await validator.ValidateAsync(request);

        if (result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }

        var userExists = await _repository.findByEmail(request.Email);
        if (userExists is not null)
        {
            throw new ConflictException(ResourceErrorMessages.EMAIL_IN_USE);
        }
    }
}