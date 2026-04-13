using Microsoft.AspNetCore.Mvc;
using MiniSIGVAL.API.DTOs.ProductoDTOs;
using MiniSIGVAL.API.Helpers;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly IProductoService _productoService;

        public ProductosController(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodos(
            [FromQuery] string? nombre,
            [FromQuery] int? categoriaId,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var productos = await _productoService.ObtenerTodosAsync(nombre, categoriaId, page, pageSize);
            return Ok(ApiResponse<PagedProductoDto>.Success(productos, "Productos obtenidos correctamente"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var producto = await _productoService.ObtenerPorIdAsync(id);

            if (producto == null)
            {
                return NotFound(ApiResponse<object>.Fail("Producto no encontrado"));
            }

            return Ok(ApiResponse<ProductoDto>.Success(producto, "Producto obtenido correctamente"));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearProductoDto dto)
        {
            var producto = await _productoService.CrearAsync(dto);

            var mensaje = producto.FueReactivado
                ? "Producto reactivado correctamente"
                : "Producto creado correctamente";

            return Ok(ApiResponse<ProductoDto>.Success(producto, mensaje));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarProductoDto dto)
        {
            var producto = await _productoService.ActualizarAsync(id, dto);

            if (producto == null)
            {
                return NotFound(ApiResponse<object>.Fail("Producto no encontrado"));
            }

            return Ok(ApiResponse<ProductoDto>.Success(producto, "Producto actualizado correctamente"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _productoService.DesactivarAsync(id);

            if (!desactivado)
            {
                return NotFound(ApiResponse<object>.Fail("Producto no encontrado"));
            }

            return Ok(ApiResponse<object>.Success(null, "Producto desactivado correctamente"));
        }
    }
}