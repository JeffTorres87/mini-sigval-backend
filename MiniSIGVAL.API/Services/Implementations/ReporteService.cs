using Microsoft.EntityFrameworkCore;
using MiniSIGVAL.API.Data;
using MiniSIGVAL.API.DTOs.ReporteDTOs;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Services.Implementations
{
    public class ReporteService : IReporteService
    {
        private readonly ApplicationDbContext _context;

        public ReporteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ReporteStockDto>> ObtenerReporteStockAsync()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .Select(p => new ReporteStockDto
                {
                    ProductoId = p.Id,
                    ProductoNombre = p.Nombre,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : string.Empty,
                    StockActual = p.StockActual,
                    EstadoStock = p.StockActual <= 5 ? "Stock Bajo" : "Stock Normal"
                })
                .ToListAsync();
        }
    }
}