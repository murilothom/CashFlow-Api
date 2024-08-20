using CashFlow.Application.UseCases.User.Register;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Users.Register;

public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserDtoBuilder.Build();
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Error_Invalid_Name(string name)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserDtoBuilder.Build();
        request.Name = name;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_NAME));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Error_Empty_Email(string email)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserDtoBuilder.Build();
        request.Email = email;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMPTY_EMAIL));
    }
    
    [Theory]
    [InlineData("foo.com")]
    [InlineData("foo@")]
    public void Error_Invalid_Email(string email)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserDtoBuilder.Build();
        request.Email = email;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_EMAIL));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Error_Empty_Password(string password)
    {
        // Arrange
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserDtoBuilder.Build();
        request.Password = password;
        
        // Act
        var result = validator.Validate(request);
        
        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));
    }
}