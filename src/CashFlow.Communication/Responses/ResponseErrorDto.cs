namespace CashFlow.Communication.Responses;

public class ResponseErrorDto
{
    public List<string> Errors { get; set; }

    public ResponseErrorDto(string errorMessage)
    {
        Errors = new List<string> { errorMessage };
    }

    public ResponseErrorDto(List<string> errors)
    {
        Errors = errors;
    }
}