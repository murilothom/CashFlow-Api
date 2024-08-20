using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Security.Cryptography;

public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        var mock = new Mock<IPasswordEncripter>();

        mock.Setup(passwordEncripter => passwordEncripter.Encrypt(It.IsAny<string>())).Returns("#a!aSDsdklasd203");
        
        return mock.Object;
    }
}