using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;

namespace WebStoreApi.Reposaitories.IReposaitories
{
    public interface IContcatsReposaitory
    {
        Task<PaginationDto<ContactDto>> GetContcatsAsync(int? page);
        Task<IEnumerable<Subject>> GetSubjectsAsync();
        Task<Subject> GetSubjectAsync(int id);
        Task<ContactDto> GetContcatByIdAsync(int id);
        Task<ContactDto> UpdateContcatAsync(int id,ContactDto Dto );
        Task<ContactDto> AddContcatAsync(ContactDto Dto );
        Task<bool> DeleteContcatAsync(int id);
    }
}
