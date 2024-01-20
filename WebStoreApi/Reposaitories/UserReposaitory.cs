using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;

namespace WebStoreApi.Reposaitories
{
    public class UserReposaitory : IUserReposaitory
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roelManager;

        public UserReposaitory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roelManager)
        {
            _userManager = userManager;
            _roelManager = roelManager;
        }

        public async Task<UsersPaginationDto> GetAllUsersAsync(int? Page)
        {
            if (Page == null || Page < 1)
            {
                Page = 1;
            }

            int PageSize = 5;
            int TotalPages = 0;

            decimal count = _userManager.Users.Count();
            TotalPages = (int)Math.Ceiling(count / PageSize);

            var users = await _userManager.Users
                .Skip((int)(Page - 1) * PageSize)
                .Take(PageSize)
                .OrderByDescending(u => u.Id).ToListAsync();
            List<UserProfileDto> userprotfiles = new List<UserProfileDto>();
            foreach (var user in users)
            {
                var usersDto = new UserProfileDto();

                usersDto.FirstName = user.FristName;
                usersDto.LastName = user.LastName;
                usersDto.userName = user.UserName;
                usersDto.Email = user.Email;
                usersDto.Phone = user.PhoneNumber;
                usersDto.Address = user.Address;
                usersDto.role = _userManager.GetRolesAsync(user).Result.ToList();

                userprotfiles.Add(usersDto);
            }

            return new UsersPaginationDto()
            {
                Profiles = userprotfiles,
                Page = Page,
                PageSize = PageSize,
                TotalPages = TotalPages

            };
        }

        public async Task<UserProfileDto> GetUserByIdAsync(string Id)
        {

            var user = await _userManager.FindByIdAsync(Id);
            var usersDto = new UserProfileDto();

            usersDto.FirstName = user.FristName;
            usersDto.LastName = user.LastName;
            usersDto.userName = user.UserName;
            usersDto.Email = user.Email;
            usersDto.Phone = user.PhoneNumber;
            usersDto.Address = user.Address;
            usersDto.role = _userManager.GetRolesAsync(user).Result.ToList();

            return usersDto;

        }

        public async Task<UserProfileDto> GetUserByUserNameAsync(string UserName)
        {

            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName.ToLower().Contains(UserName.ToLower()));
            }
            var usersDto = new UserProfileDto();

            usersDto.FirstName = user.FristName;
            usersDto.LastName = user.LastName;
            usersDto.userName = user.UserName;
            usersDto.Email = user.Email;
            usersDto.Phone = user.PhoneNumber;
            usersDto.Address = user.Address;
            usersDto.role = _userManager.GetRolesAsync(user).Result.ToList();

            return usersDto;

        }
    }
}
