using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CommonTestUtilities.Requests;
using FluentAssertions;

namespace Validators.Test.Expenses.Register;

public class ExpenseValidatorTest
{
    [Fact]
    public void Success()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseDtoBuilder.Build();

        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void Error_Invalid_Title(string title)
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseDtoBuilder.Build();
        request.Title = title;
        
        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }
    
    [Fact]
    public void Error_Date_Future()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseDtoBuilder.Build();
        request.Date = DateTime.UtcNow.AddDays(1);
        
        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPANSE_CANNOT_BE_IN_THE_FUTURE));
    }
        
    [Fact]
    public void Error_Invalid_Payment_Type()
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseDtoBuilder.Build();
        request.PaymentType = (PaymentType)999;
        
        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PAYMENT_TYPE));
    }
            
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-200)]
    public void Error_Invalid_Amount(decimal amount)
    {
        // Arrange
        var validator = new ExpenseValidator();
        var request = RequestRegisterExpenseDtoBuilder.Build();
        request.Amount = amount;
        
        // Act
        var result = validator.Validate(request);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCount(1).And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
}