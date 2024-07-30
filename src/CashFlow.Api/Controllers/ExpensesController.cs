using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterExpenseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] RegisterExpenseUseCase useCase,
            [FromBody] RequestRegisterExpenseDto request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }
    }
}
