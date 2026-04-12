namespace MiniSIGVAL.API.DTOs.StockDTOs
{
    public class MovimientoStockDto
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }

        public string ProductoNombre { get; set; } = string.Empty;

        public string TipoMovimiento { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public string? Observacion { get; set; }

        public DateTime FechaMovimiento { get; set; }
    }
}