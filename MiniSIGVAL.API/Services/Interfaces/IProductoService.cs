using MiniSIGVAL.API.DTOs.ProductoDTOs;

namespace MiniSIGVAL.API.Services.Interfaces
{
    public interface IProductoService
    {
        Task<PagedProductoDto> ObtenerTodosAsync(string? nombre = null, int? categoriaId = null, int page = 1, int pageSize = 10);
        Task<ProductoDto?> ObtenerPorIdAsync(int id);
        Task<ProductoDto> CrearAsync(CrearProductoDto dto);
        Task<ProductoDto?> ActualizarAsync(int id, ActualizarProductoDto dto);
        Task<bool> DesactivarAsync(int id);
    }
}