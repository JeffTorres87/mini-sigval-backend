// MiniSIGVAL.API/DTOs/StockDTOs/SalidaStockDto.cs

using System.ComponentModel.DataAnnotations;

namespace MiniSIGVAL.API.DTOs.StockDTOs
{
    public class SalidaStockDto
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int ProductoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [MaxLength(250, ErrorMessage = "La observación no puede exceder 250 caracteres.")]
        public string? Observacion { get; set; }
    }
}