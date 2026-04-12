namespace MiniSIGVAL.API.DTOs.ReporteDTOs
{
    public class ReporteStockDto
    {
        public int ProductoId { get; set; }

        public string ProductoNombre { get; set; } = string.Empty;

        public string CategoriaNombre { get; set; } = string.Empty;

        public int StockActual { get; set; }

        public string EstadoStock { get; set; } = string.Empty;
    }
}