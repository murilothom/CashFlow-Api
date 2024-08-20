using CashFlow.Application.UseCases.Login;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using CommonTestUtilities.Security.Tokens;
using FluentAssertions;

namespace UseCases.Test.Login;

public class LoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestLoginDtoBuilder.Build();
        request.Email = "existent@email.com";
        var useCase = CreateUseCase(request.Email, request.Password);

        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Existent User");
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Wrong_Email()
    {
        // Arrange
        var request = RequestLoginDtoBuilder.Build();
        var useCase = CreateUseCase(request.Email, request.Password);

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var result = await act.Should().ThrowAsync<UnauthorizedException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.LOGIN_FAILED));
    }

    [Fact]
    public async Task Error_Wrong_Password()
    {
        // Arrange
        var request = RequestLoginDtoBuilder.Build();
        request.Email = "existent@email.com";
        var useCase = CreateUseCase(request.Email);

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var result = await act.Should().ThrowAsync<UnauthorizedException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.LOGIN_FAILED));
    }

    private LoginUseCase CreateUseCase(string email, string? password = null)
    {
        var repository = new UsersRepositoryBuilder();
        repository.GetByEmail(email);

        var passwordEncripter = new PasswordEncrypterBuilder();
        if (string.IsNullOrWhiteSpace(password) == false)
        {
            passwordEncripter.Verify(password);
        }
        
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        return new LoginUseCase(repository.Build(), passwordEncripter.Build(), accessTokenGenerator);
    }
}