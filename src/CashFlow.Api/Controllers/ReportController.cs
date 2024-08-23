using System.Net.Mime;
using CashFlow.Application.UseCases.Expenses.Reports.Excel;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf;
using CashFlow.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Role.ADMIN)]
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public async Task<IActionResult> GetExcel(
            [FromServices] IGenerateExpensesReportExcelUseCase useCase,
            [FromQuery] DateOnly month)
        {
            var file = await useCase.Execute(month);

            return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
        }
        
        [HttpGet("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetExcel(
            [FromServices] IGenerateExpensesReportPdfUseCase useCase,
            [FromQuery] DateOnly month)
        {
            var file = await useCase.Execute(month);

            return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
        }
    }
}
