using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseDto Execute(RequestRegisterExpenseDto request)
    {
        // TODO: Validations
        
        return new ResponseRegisterExpenseDto();
    }
}