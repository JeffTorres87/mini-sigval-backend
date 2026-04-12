// MiniSIGVAL.API/Models/Categoria.cs

using System.ComponentModel.DataAnnotations;

namespace MiniSIGVAL.API.Models
{
    public class Categoria
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la categoría es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la categoría no puede exceder 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres.")]
        public string? Descripcion { get; set; }

        public bool Activo { get; set; } = true;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}