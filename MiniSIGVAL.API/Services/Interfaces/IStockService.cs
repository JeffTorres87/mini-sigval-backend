using MiniSIGVAL.API.DTOs.StockDTOs;

namespace MiniSIGVAL.API.Services.Interfaces
{
    public interface IStockService
    {
        Task RegistrarEntradaAsync(EntradaStockDto dto);
        Task RegistrarSalidaAsync(SalidaStockDto dto);
        Task<List<MovimientoStockDto>> ObtenerMovimientosAsync();
        Task<List<MovimientoStockDto>> ObtenerMovimientosPorProductoAsync(int productoId);
    }
}