using Microsoft.EntityFrameworkCore;
using MiniSIGVAL.API.Data;
using MiniSIGVAL.API.DTOs.StockDTOs;
using MiniSIGVAL.API.Models;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Services.Implementations
{
    public class StockService : IStockService
    {
        private readonly ApplicationDbContext _context;

        public StockService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarEntradaAsync(EntradaStockDto dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == dto.ProductoId && p.Activo);

            if (producto == null)
            {
                throw new InvalidOperationException("Producto no encontrado o inactivo.");
            }

            producto.StockActual += dto.Cantidad;

            var movimiento = new MovimientoStock
            {
                ProductoId = producto.Id,
                Cantidad = dto.Cantidad,
                TipoMovimiento = "ENTRADA",
                Observacion = dto.Observacion?.Trim(),
                FechaMovimiento = DateTime.Now
            };

            _context.MovimientosStock.Add(movimiento);

            await _context.SaveChangesAsync();
        }

        public async Task RegistrarSalidaAsync(SalidaStockDto dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == dto.ProductoId && p.Activo);

            if (producto == null)
            {
                throw new InvalidOperationException("Producto no encontrado o inactivo.");
            }

            if (producto.StockActual < dto.Cantidad)
            {
                throw new InvalidOperationException("Stock insuficiente para realizar la salida.");
            }

            producto.StockActual -= dto.Cantidad;

            var movimiento = new MovimientoStock
            {
                ProductoId = producto.Id,
                Cantidad = dto.Cantidad,
                TipoMovimiento = "SALIDA",
                Observacion = dto.Observacion?.Trim(),
                FechaMovimiento = DateTime.Now
            };

            _context.MovimientosStock.Add(movimiento);

            await _context.SaveChangesAsync();
        }

        public async Task<List<MovimientoStockDto>> ObtenerMovimientosAsync()
        {
            return await _context.MovimientosStock
                .Include(m => m.Producto)
                .OrderByDescending(m => m.FechaMovimiento)
                .Select(m => new MovimientoStockDto
                {
                    Id = m.Id,
                    ProductoId = m.ProductoId,
                    ProductoNombre = m.Producto != null ? m.Producto.Nombre : string.Empty,
                    TipoMovimiento = m.TipoMovimiento,
                    Cantidad = m.Cantidad,
                    Observacion = m.Observacion,
                    FechaMovimiento = m.FechaMovimiento
                })
                .ToListAsync();
        }

        public async Task<List<MovimientoStockDto>> ObtenerMovimientosPorProductoAsync(int productoId)
        {
            var productoExiste = await _context.Productos
                .AnyAsync(p => p.Id == productoId);

            if (!productoExiste)
            {
                throw new InvalidOperationException("Producto no encontrado.");
            }

            return await _context.MovimientosStock
                .Include(m => m.Producto)
                .Where(m => m.ProductoId == productoId)
                .OrderByDescending(m => m.FechaMovimiento)
                .Select(m => new MovimientoStockDto
                {
                    Id = m.Id,
                    ProductoId = m.ProductoId,
                    ProductoNombre = m.Producto != null ? m.Producto.Nombre : string.Empty,
                    TipoMovimiento = m.TipoMovimiento,
                    Cantidad = m.Cantidad,
                    Observacion = m.Observacion,
                    FechaMovimiento = m.FechaMovimiento
                })
                .ToListAsync();
        }
    }
}