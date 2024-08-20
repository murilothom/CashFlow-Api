using CashFlow.Application.UseCases.User.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security.Cryptography;
using CommonTestUtilities.Security.Tokens;
using FluentAssertions;

namespace UseCases.Test.User.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestRegisterUserDtoBuilder.Build();
        var useCase = CreateUseCase();
        
        // Act
        var result = await useCase.Execute(request);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Error_Empty_Name()
    {
        // Arrange
        var request = RequestRegisterUserDtoBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        // Act
        var act = async () => await useCase.Execute(request);

        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMPTY_NAME));
    }

    [Fact]
    public async Task Error_Email_In_Use()
    {
        // Arrange
        var request = RequestRegisterUserDtoBuilder.Build();
        var useCase = CreateUseCase(request.Email);
        
        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<ConflictException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_IN_USE));
    }

    private RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var repository = new UsersRepositoryBuilder();
        var passwordEncripter = PasswordEncripterBuilder.Build();
        var accessTokenGenerator = JwtTokenGeneratorBuilder.Build();

        if (string.IsNullOrWhiteSpace(email) == false)
        {
            repository.GetByEmail(email);
        }
        
        return new RegisterUserUseCase(repository.Build(), mapper, passwordEncripter, accessTokenGenerator);
    }
} 