using Microsoft.EntityFrameworkCore;
using MiniSIGVAL.API.Data;
using MiniSIGVAL.API.DTOs.ProductoDTOs;
using MiniSIGVAL.API.Models;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Services.Implementations
{
    public class ProductoService : IProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedProductoDto> ObtenerTodosAsync(string? nombre = null, int? categoriaId = null, int page = 1, int pageSize = 10)
        {
            if (page < 1)
            {
                page = 1;
            }

            if (pageSize < 1)
            {
                pageSize = 10;
            }

            if (pageSize > 100)
            {
                pageSize = 100;
            }

            var query = _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Activo)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(nombre))
            {
                var nombreBuscado = nombre.Trim().ToLower();
                query = query.Where(p => p.Nombre.ToLower().Contains(nombreBuscado));
            }

            if (categoriaId.HasValue)
            {
                query = query.Where(p => p.CategoriaId == categoriaId.Value);
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    StockActual = p.StockActual,
                    Activo = p.Activo,
                    FechaCreacion = p.FechaCreacion,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria != null ? p.Categoria.Nombre : string.Empty,
                    FueReactivado = false
                })
                .ToListAsync();

            return new PagedProductoDto
            {
                Items = items,
                Total = total,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<ProductoDto?> ObtenerPorIdAsync(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (producto == null)
            {
                return null;
            }

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                StockActual = producto.StockActual,
                Activo = producto.Activo,
                FechaCreacion = producto.FechaCreacion,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.Categoria != null ? producto.Categoria.Nombre : string.Empty,
                FueReactivado = false
            };
        }

        public async Task<ProductoDto> CrearAsync(CrearProductoDto dto)
        {
            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == dto.CategoriaId && c.Activo);

            if (!categoriaExiste)
            {
                throw new InvalidOperationException("La categoría seleccionada no existe o está inactiva.");
            }

            var nombreNormalizado = dto.Nombre.Trim().ToLower();

            var productoActivoDuplicado = await _context.Productos
                .AnyAsync(p => p.Nombre.ToLower() == nombreNormalizado && p.Activo);

            if (productoActivoDuplicado)
            {
                throw new InvalidOperationException("Ya existe un producto activo con ese nombre.");
            }

            var productoInactivo = await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Nombre.ToLower() == nombreNormalizado && !p.Activo);

            if (productoInactivo != null)
            {
                productoInactivo.Nombre = dto.Nombre.Trim();
                productoInactivo.Descripcion = dto.Descripcion?.Trim();
                productoInactivo.Precio = dto.Precio;
                productoInactivo.StockActual = dto.StockActual;
                productoInactivo.CategoriaId = dto.CategoriaId;
                productoInactivo.Activo = true;

                await _context.SaveChangesAsync();

                var categoriaReactivada = await _context.Categorias
                    .FirstAsync(c => c.Id == productoInactivo.CategoriaId);

                return new ProductoDto
                {
                    Id = productoInactivo.Id,
                    Nombre = productoInactivo.Nombre,
                    Descripcion = productoInactivo.Descripcion,
                    Precio = productoInactivo.Precio,
                    StockActual = productoInactivo.StockActual,
                    Activo = productoInactivo.Activo,
                    FechaCreacion = productoInactivo.FechaCreacion,
                    CategoriaId = productoInactivo.CategoriaId,
                    CategoriaNombre = categoriaReactivada.Nombre,
                    FueReactivado = true
                };
            }

            var producto = new Producto
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                Precio = dto.Precio,
                StockActual = dto.StockActual,
                Activo = true,
                FechaCreacion = DateTime.Now,
                CategoriaId = dto.CategoriaId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var categoria = await _context.Categorias
                .FirstAsync(c => c.Id == producto.CategoriaId);

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                StockActual = producto.StockActual,
                Activo = producto.Activo,
                FechaCreacion = producto.FechaCreacion,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = categoria.Nombre,
                FueReactivado = false
            };
        }

        public async Task<ProductoDto?> ActualizarAsync(int id, ActualizarProductoDto dto)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return null;
            }

            var categoriaExiste = await _context.Categorias
                .AnyAsync(c => c.Id == dto.CategoriaId && c.Activo);

            if (!categoriaExiste)
            {
                throw new InvalidOperationException("La categoría seleccionada no existe o está inactiva.");
            }

            var nombreNormalizado = dto.Nombre.Trim().ToLower();

            var existeOtroProductoActivo = await _context.Productos
                .AnyAsync(p => p.Id != id && p.Nombre.ToLower() == nombreNormalizado && p.Activo);

            if (existeOtroProductoActivo)
            {
                throw new InvalidOperationException("Ya existe otro producto activo con ese nombre.");
            }

            producto.Nombre = dto.Nombre.Trim();
            producto.Descripcion = dto.Descripcion?.Trim();
            producto.Precio = dto.Precio;
            producto.StockActual = dto.StockActual;
            producto.CategoriaId = dto.CategoriaId;

            await _context.SaveChangesAsync();

            var categoria = await _context.Categorias
                .FirstAsync(c => c.Id == producto.CategoriaId);

            return new ProductoDto
            {
                Id = producto.Id,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                StockActual = producto.StockActual,
                Activo = producto.Activo,
                FechaCreacion = producto.FechaCreacion,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = categoria.Nombre,
                FueReactivado = false
            };
        }

        public async Task<bool> DesactivarAsync(int id)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id && p.Activo);

            if (producto == null)
            {
                return false;
            }

            producto.Activo = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}