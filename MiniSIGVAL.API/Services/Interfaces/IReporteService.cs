using MiniSIGVAL.API.DTOs.ReporteDTOs;

namespace MiniSIGVAL.API.Services.Interfaces
{
    public interface IReporteService
    {
        Task<List<ReporteStockDto>> ObtenerReporteStockAsync();
    }
}