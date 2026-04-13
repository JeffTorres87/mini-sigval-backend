using Microsoft.EntityFrameworkCore;
using MiniSIGVAL.API.Data;
using MiniSIGVAL.API.DTOs.CategoriaDTOs;
using MiniSIGVAL.API.Models;
using MiniSIGVAL.API.Services.Interfaces;

namespace MiniSIGVAL.API.Services.Implementations
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoriaDto>> ObtenerTodasAsync()
        {
            return await _context.Categorias
                .Where(c => c.Activo)
                .OrderBy(c => c.Id)
                .Select(c => new CategoriaDto
                {
                    Id = c.Id,
                    Nombre = c.Nombre,
                    Descripcion = c.Descripcion,
                    Activo = c.Activo,
                    FechaCreacion = c.FechaCreacion
                })
                .ToListAsync();
        }

        public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

            if (categoria == null)
            {
                return null;
            }

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto)
        {
            var nombreNormalizado = dto.Nombre.Trim().ToLower();

            var existeCategoriaActiva = await _context.Categorias
                .AnyAsync(c => c.Nombre.ToLower() == nombreNormalizado && c.Activo);

            if (existeCategoriaActiva)
            {
                throw new InvalidOperationException("Ya existe una categoría activa con ese nombre.");
            }

            var categoriaInactiva = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Nombre.ToLower() == nombreNormalizado && !c.Activo);

            if (categoriaInactiva != null)
            {
                categoriaInactiva.Nombre = dto.Nombre.Trim();
                categoriaInactiva.Descripcion = dto.Descripcion?.Trim();
                categoriaInactiva.Activo = true;
                await _context.SaveChangesAsync();

                return new CategoriaDto
                {
                    Id = categoriaInactiva.Id,
                    Nombre = categoriaInactiva.Nombre,
                    Descripcion = categoriaInactiva.Descripcion,
                    Activo = categoriaInactiva.Activo,
                    FechaCreacion = categoriaInactiva.FechaCreacion
                };
            }

            var categoria = new Categoria
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                Activo = true,
                FechaCreacion = DateTime.Now
            };

            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        public async Task<CategoriaDto?> ActualizarAsync(int id, ActualizarCategoriaDto dto)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id);

            if (categoria == null)
            {
                return null;
            }

            var nombreNormalizado = dto.Nombre.Trim().ToLower();

            // 1. Verificar si existe otra categoría ACTIVA con ese nombre
            var existeActiva = await _context.Categorias
                .AnyAsync(c => c.Id != id && c.Nombre.ToLower() == nombreNormalizado && c.Activo);

            if (existeActiva)
            {
                throw new InvalidOperationException("Ya existe otra categoría activa con ese nombre.");
            }

            // 2. Verificar si existe una categoría INACTIVA con ese nombre
            var categoriaInactiva = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id != id && c.Nombre.ToLower() == nombreNormalizado && !c.Activo);

            if (categoriaInactiva != null)
            {
                // Reactivar la existente
                categoriaInactiva.Activo = true;

                // Desactivar la actual
                categoria.Activo = false;

                await _context.SaveChangesAsync();

                return new CategoriaDto
                {
                    Id = categoriaInactiva.Id,
                    Nombre = categoriaInactiva.Nombre,
                    Descripcion = categoriaInactiva.Descripcion,
                    Activo = categoriaInactiva.Activo,
                    FechaCreacion = categoriaInactiva.FechaCreacion
                };
            }

            // 3. Actualización normal
            categoria.Nombre = dto.Nombre.Trim();
            categoria.Descripcion = dto.Descripcion?.Trim();

            await _context.SaveChangesAsync();

            return new CategoriaDto
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo,
                FechaCreacion = categoria.FechaCreacion
            };
        }

        public async Task<bool> DesactivarAsync(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.Id == id && c.Activo);

            if (categoria == null)
            {
                return false;
            }

            var tieneProductosActivosAsociados = await _context.Productos
                .AnyAsync(p => p.CategoriaId == id && p.Activo);

            if (tieneProductosActivosAsociados)
            {
                throw new InvalidOperationException("No se puede desactivar la categoría porque tiene productos activos asociados.");
            }

            categoria.Activo = false;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}