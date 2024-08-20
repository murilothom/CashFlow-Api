using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Login;

public class LoginUseCase : ILoginUseCase
{
    private readonly IUsersRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public LoginUseCase(
        IUsersRepository repository,
        IPasswordEncripter passwordEncripterEncripter,
        IAccessTokenGenerator tokenGenerator)
    {
        _repository = repository;
        _passwordEncripter = passwordEncripterEncripter;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisterUserDto> Execute(RequestLoginDto request)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user is null)
        {
            throw new UnauthorizedException(ResourceErrorMessages.LOGIN_FAILED);
        }
        
        var isPasswordValid = _passwordEncripter.Verify(request.Password, user.Password);
        if (isPasswordValid == false)
        {
            throw new UnauthorizedException(ResourceErrorMessages.LOGIN_FAILED);
        }
        
        var accessToken = _tokenGenerator.Generate(user);
        
        return new ResponseRegisterUserDto
        {
            Name = user.Name,
            Token = accessToken,
        };
    }
}