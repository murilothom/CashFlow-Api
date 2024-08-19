using CashFlow.Application.UseCases.Expenses.DeleteById;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.UpdateById;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseExpensesDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllExpenses([FromServices] IGetAllExpensesUseCase useCase)
        {
            var response = await useCase.Execute();

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:long}")]
        [ProducesResponseType(typeof(ResponseExpenseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(
            [FromServices] IGetExpenseByIdUseCase useCase,
            [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterExpenseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(
            [FromServices] IRegisterExpenseUseCase useCase,
            [FromBody] RegisterExpenseDto request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }
        
        [HttpPut]
        [Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateById(
            [FromServices] IUpdateExpenseByIdUseCase useCase,
            [FromRoute] long id,
            [FromBody] RegisterExpenseDto request)
        {
            await useCase.Execute(id, request);

            return NoContent();
        }
        
        [HttpDelete]
        [Route("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(
            [FromServices] IDeleteExpenseByIdUseCase useCase,
            [FromRoute] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }
    }
}
