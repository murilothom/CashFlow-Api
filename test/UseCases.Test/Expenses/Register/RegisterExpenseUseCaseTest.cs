using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services.LoggedUser;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register;

public class RegisterExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder.Build();
        var request = RequestRegisterExpenseDtoBuilder.Build();
        var useCase = CreateUseCase(user);

        // Act
        var result = await useCase.Execute(request);
        
        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }
    
    [Fact]
    public async Task Error_Empty_Title()
    {
        // Arrange
        var user = UserBuilder.Build();
        var request = RequestRegisterExpenseDtoBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(user);

        // Act
        var act = async () => await useCase.Execute(request);
        
        // Assert
        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();
        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }

    private RegisterExpenseUseCase CreateUseCase(CashFlow.Domain.Entities.User user)
    {
        var repository = ExpensesRepositoryBuilder.Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new RegisterExpenseUseCase(repository, mapper, loggedUser);
    }
}