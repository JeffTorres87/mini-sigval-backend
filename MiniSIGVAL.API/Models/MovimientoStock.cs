// MiniSIGVAL.API/Models/MovimientoStock.cs

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSIGVAL.API.Models
{
    public class MovimientoStock
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
        [MaxLength(10, ErrorMessage = "El tipo de movimiento no puede exceder 10 caracteres.")]
        public string TipoMovimiento { get; set; } = string.Empty; // ENTRADA o SALIDA

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [MaxLength(250, ErrorMessage = "La observación no puede exceder 250 caracteres.")]
        public string? Observacion { get; set; }

        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        public int ProductoId { get; set; }

        public Producto? Producto { get; set; }
    }
}