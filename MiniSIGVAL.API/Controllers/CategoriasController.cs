using Microsoft.AspNetCore.Mvc;
using MiniSIGVAL.API.DTOs.CategoriaDTOs;
using MiniSIGVAL.API.Helpers;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();
            return Ok(ApiResponse<List<CategoriaDto>>.Success(categorias, "Categorías obtenidas correctamente"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var categoria = await _categoriaService.ObtenerPorIdAsync(id);

            if (categoria == null)
            {
                return NotFound(ApiResponse<object>.Fail("Categoría no encontrada"));
            }

            return Ok(ApiResponse<CategoriaDto>.Success(categoria, "Categoría obtenida correctamente"));
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto dto)
        {
            var nuevaCategoria = await _categoriaService.CrearAsync(dto);
            return Ok(ApiResponse<CategoriaDto>.Success(nuevaCategoria, "Categoría creada correctamente"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ActualizarCategoriaDto dto)
        {
            var categoria = await _categoriaService.ActualizarAsync(id, dto);

            if (categoria == null)
            {
                return NotFound(ApiResponse<object>.Fail("Categoría no encontrada"));
            }

            return Ok(ApiResponse<CategoriaDto>.Success(categoria, "Categoría actualizada correctamente"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Desactivar(int id)
        {
            var desactivado = await _categoriaService.DesactivarAsync(id);

            if (!desactivado)
            {
                return NotFound(ApiResponse<object>.Fail("Categoría no encontrada"));
            }

            return Ok(ApiResponse<object>.Success(null, "Categoría desactivada correctamente"));
        }
    }
}