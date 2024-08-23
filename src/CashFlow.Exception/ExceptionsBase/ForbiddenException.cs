using System.Net;

namespace CashFlow.Exception.ExceptionsBase;

public class ForbiddenException: CashFlowException
{
    public ForbiddenException(string message) : base(message) { }
    
    public override int StatusCode => (int)HttpStatusCode.Forbidden;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}