using CashFlow.Application.UseCases.User;
using CommonTestUtilities.Requests;
using FluentAssertions;
using FluentValidation;

namespace Validators.Test.Users;

public class PasswordValidatorTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    [InlineData("abcdef")]
    [InlineData("abcdef12")]
    [InlineData("abcdefgh")]
    [InlineData("ABCDEFGH")]
    [InlineData("Abcdefg1")]
    [InlineData("abcdefg1!")]
    public void Error_Invalid_Password(string password)
    {
        // Arrange
        var validator = new PasswordValidator<RequestRegisterUserDtoBuilder>();
        
        // Act
        var result = validator.IsValid(
            new ValidationContext<RequestRegisterUserDtoBuilder>(new RequestRegisterUserDtoBuilder()), password);
        
        // Assert
        result.Should().BeFalse();
    }
}