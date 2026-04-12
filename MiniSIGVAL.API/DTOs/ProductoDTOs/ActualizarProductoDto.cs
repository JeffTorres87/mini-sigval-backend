// MiniSIGVAL.API/DTOs/ProductoDTOs/ActualizarProductoDto.cs

using System.ComponentModel.DataAnnotations;

namespace MiniSIGVAL.API.DTOs.ProductoDTOs
{
    public class ActualizarProductoDto
    {
        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre del producto no puede exceder 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "La descripción no puede exceder 500 caracteres.")]
        public string? Descripcion { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0.")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock actual no puede ser negativo.")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoriaId { get; set; }

        public bool Activo { get; set; }
    }
}