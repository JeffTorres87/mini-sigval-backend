using Microsoft.AspNetCore.Mvc;
using MiniSIGVAL.API.DTOs.StockDTOs;
using MiniSIGVAL.API.Helpers;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpPost("entrada")]
        public async Task<IActionResult> RegistrarEntrada([FromBody] EntradaStockDto dto)
        {
            await _stockService.RegistrarEntradaAsync(dto);
            return Ok(ApiResponse<object>.Success(null, "Entrada de stock registrada correctamente"));
        }

        [HttpPost("salida")]
        public async Task<IActionResult> RegistrarSalida([FromBody] SalidaStockDto dto)
        {
            await _stockService.RegistrarSalidaAsync(dto);
            return Ok(ApiResponse<object>.Success(null, "Salida de stock registrada correctamente"));
        }

        [HttpGet("movimientos")]
        public async Task<IActionResult> ObtenerMovimientos()
        {
            var movimientos = await _stockService.ObtenerMovimientosAsync();
            return Ok(ApiResponse<List<MovimientoStockDto>>.Success(movimientos, "Movimientos de stock obtenidos correctamente"));
        }

        [HttpGet("movimientos/{productoId}")]
        public async Task<IActionResult> ObtenerMovimientosPorProducto(int productoId)
        {
            var movimientos = await _stockService.ObtenerMovimientosPorProductoAsync(productoId);
            return Ok(ApiResponse<List<MovimientoStockDto>>.Success(movimientos, "Movimientos del producto obtenidos correctamente"));
        }
    }
}