using CashFlow.Application.UseCases.Login;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromServices] ILoginUseCase useCase,
            [FromBody] RequestLoginDto request)
        {
            var response = await useCase.Execute(request);

            return Ok(response);
        }
    }
}
