using WebStoreApi.Models.DTOS;

namespace WebStoreApi.Reposaitories.IReposaitories
{
    public interface IUserReposaitory 
    {
        Task<UsersPaginationDto> GetAllUsersAsync(int? Page);
         
        Task<UserProfileDto> GetUserByIdAsync(string Id);
        Task<UserProfileDto> GetUserByUserNameAsync(string UserName);
    }
}
