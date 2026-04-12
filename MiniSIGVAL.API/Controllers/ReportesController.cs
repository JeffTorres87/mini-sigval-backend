using Microsoft.AspNetCore.Mvc;
using MiniSIGVAL.API.DTOs.ReporteDTOs;
using MiniSIGVAL.API.Helpers;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        [HttpGet("stock")]
        public async Task<IActionResult> ObtenerReporteStock()
        {
            var reporte = await _reporteService.ObtenerReporteStockAsync();
            return Ok(ApiResponse<List<ReporteStockDto>>.Success(reporte, "Reporte de stock obtenido correctamente"));
        }
    }
}