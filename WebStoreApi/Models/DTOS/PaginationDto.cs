namespace WebStoreApi.Models.DTOS
{
    public class PaginationDto<T>
    {
        public IEnumerable<T> Contacts { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int? Page { get; set; }

    }
}
