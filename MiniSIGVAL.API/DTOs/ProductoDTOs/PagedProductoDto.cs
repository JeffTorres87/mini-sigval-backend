namespace MiniSIGVAL.API.DTOs.ProductoDTOs
{
    public class PagedProductoDto
    {
        public List<ProductoDto> Items { get; set; } = new();
        public int Total { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}