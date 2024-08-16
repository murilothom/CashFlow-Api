using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class UnauthorizedException: CashFlowException
{
    public UnauthorizedException(string message) : base(message) { }
    
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}