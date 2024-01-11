using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;

namespace WebStoreApi.Reposaitories.IReposaitories
{
    public interface IAccountReposaitory
    {
        Task<bool> CheckPasswordAsync(ApplicationUser user, string Password);
        Task<ApplicationUser> FindUserById(string id);
        Task<UserProfileDto> GetProfile(string id);
        Task<bool> IsEmailNotRegistered(string email);
        Task<bool> IsUserNameNotRegistered(string UserName);
        Task<LoginResponse> Login(string Email, string password);
        Task<RegisterResponse> Register (UserDto user);
        Task<string> UpdatePassword(UpdatePasswordDto updatedpassword, ApplicationUser user);
        Task<UpdateUserProfileDto> UpdateProfile(UpdateUserProfileDto updatedUser, string id);
    }
}
