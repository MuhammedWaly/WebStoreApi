namespace WebStoreApi.Models.DTOS
{
    public class ProductsPaginationDto<T>
    {

        public IEnumerable<T> Products { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int? Page { get; set; }
    }
}
