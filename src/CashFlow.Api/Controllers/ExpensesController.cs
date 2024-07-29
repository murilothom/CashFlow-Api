using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register(
            [FromServices] RegisterExpenseUseCase useCase,
            [FromBody] RequestRegisterExpenseDto request)
        {
            var response = useCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
