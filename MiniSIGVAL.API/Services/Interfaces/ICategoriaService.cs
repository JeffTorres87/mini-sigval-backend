using MiniSIGVAL.API.DTOs.CategoriaDTOs;

namespace MiniSIGVAL.API.Services.Interfaces
{
    public interface ICategoriaService
    {
        Task<List<CategoriaDto>> ObtenerTodasAsync();
        Task<CategoriaDto?> ObtenerPorIdAsync(int id);
        Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto);
        Task<CategoriaDto?> ActualizarAsync(int id, ActualizarCategoriaDto dto);
        Task<bool> DesactivarAsync(int id);
    }
}