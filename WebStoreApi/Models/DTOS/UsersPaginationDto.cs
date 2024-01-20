namespace WebStoreApi.Models.DTOS
{
    public class UsersPaginationDto
    {
        public List<UserProfileDto> Profiles { get; set; }
        public int? Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}
