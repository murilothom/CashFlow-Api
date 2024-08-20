using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommonTestUtilities.Security.Cryptography;

public class PasswordEncrypterBuilder
{
    private readonly Mock<IPasswordEncrypter> _mock;
    public PasswordEncrypterBuilder()
    {
        _mock = new Mock<IPasswordEncrypter>();

        _mock.Setup(passwordEncrypter => passwordEncrypter.Encrypt(It.IsAny<string>())).Returns("#a!aSDsdklasd203");
    }

    public void Verify(string password)
    {
        _mock.Setup(passwordEncrypter => passwordEncrypter.Verify(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordEncrypter Build() => _mock.Object;
}