// MiniSIGVAL.API/DTOs/CategoriaDTOs/CrearCategoriaDto.cs

using System.ComponentModel.DataAnnotations;

namespace MiniSIGVAL.API.DTOs.CategoriaDTOs
{
    public class CrearCategoriaDto
    {
        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la categoría no puede exceder 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres.")]
        public string? Descripcion { get; set; }
    }
}