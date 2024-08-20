using Bogus;
using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CommonTestUtilities.Security.Cryptography;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        var passwordEncripter = new PasswordEncrypterBuilder().Build();
        
        return new Faker<User>()
            .RuleFor(user => user.Id, _ => Guid.NewGuid())
            .RuleFor(user => user.Name, faker => faker.Name.FirstName())
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (_, user) => passwordEncripter.Encrypt(user.Password));
    }
}