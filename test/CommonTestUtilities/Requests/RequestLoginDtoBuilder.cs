using Bogus;
using CashFlow.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestLoginDtoBuilder
{
    public static RequestLoginDto Build()
    {
        return new Faker<RequestLoginDto>()
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email())
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!1aA"));
    }
}