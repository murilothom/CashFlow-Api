using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUsersRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    
    public RegisterUserUseCase(
        IUsersRepository repository,
        IMapper mapper,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
    }
    public async Task<ResponseRegisterUserDto> Execute(RequestRegisterUserDto request)
    {
        await Validate(request);
        
        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);

        await _repository.Add(user);
        
        var accessToken = _tokenGenerator.Generate(user);

        return new ResponseRegisterUserDto
        {
            Name = user.Name,
            Token = accessToken,
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

        var userExists = await _repository.getByEmail(request.Email);
        if (userExists is not null)
        {
            throw new ConflictException(ResourceErrorMessages.EMAIL_IN_USE);
        }
    }
}