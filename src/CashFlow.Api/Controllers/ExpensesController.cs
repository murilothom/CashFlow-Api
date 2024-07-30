using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
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
        [HttpGet]
        [ProducesResponseType(typeof(ResponseExpensesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExpenses([FromServices] GetAllExpensesUseCase useCase)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        [ProducesResponseType(typeof(ResponseExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] GetExpenseByIdUseCase useCase,
            [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }
        
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
