using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebStoreApi.Data;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;

namespace WebStoreApi.Reposaitories
{
    public class AccountReposaitory : IAccountReposaitory
    {
        private readonly IConfiguration _configuration;

        private readonly UserManager<ApplicationUser> _userManger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;


        public AccountReposaitory(IConfiguration configuration, UserManager<ApplicationUser> userManger, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _configuration = configuration;
            _userManger = userManger;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        private string CreateJwtToken(ApplicationUser user)
        {
            string strkey = _configuration["JwtSettings:key"]!;

            var roles = _userManger.GetRolesAsync(user).Result;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(strkey);

            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault()),
                    new Claim("UId",user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(TokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }


        public async Task<RegisterResponse> Register(UserDto user)
        {
            var Newuser = new ApplicationUser()
            {
                FristName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.Phone,
                UserName = user.userName,
                PasswordHash = user.Password,
                CreatedAt = DateTime.Now,
                Address = user.Address

            };

            var result = await _userManger.CreateAsync(Newuser, user.Password);
            if (result.Succeeded)
            {

                await _userManger.AddToRoleAsync(Newuser, "admin");

                var jwt = CreateJwtToken(Newuser);
                var profile = new UserProfileDto()
                {

                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    userName = user.userName,
                    CreatedAt = Newuser.CreatedAt,
                    Address = user.Address
                };
                return new RegisterResponse()
                {
                    profile = profile,
                    Token = jwt

                };

            }
            return new RegisterResponse();
        }



        public async Task<LoginResponse> Login(string Email, string password)
        {
            var profile = new UserProfileDto();
            var user = await _userManger.FindByEmailAsync(Email);
            if (user == null || !await _userManger.CheckPasswordAsync(user!, password))
            {
                return new LoginResponse() { Message = "Invalid Email or password" };
            }
            var RolesList = await _userManger.GetRolesAsync(user);
            var jwt = CreateJwtToken(user!);



            profile.FirstName = user.FristName;
            profile.LastName = user.LastName;
            profile.Email = user.Email;
            profile.Phone = user.PhoneNumber;
            profile.userName = user.UserName;
            profile.Address = user.Address;
            profile.role = RolesList.ToList();

            return new LoginResponse()
            {
                profile = profile,
                Token = jwt

            };


        }


        public async Task<bool> IsEmailNotRegistered(string email)
        {
            var User = await _userManger.FindByEmailAsync(email);
            if (User == null)
                return true;
            return false;

        }

        public async Task<bool> IsUserNameNotRegistered(string UserName)
        {
            var User = await _userManger.FindByNameAsync(UserName);
            if (User == null)
                return true;
            return false;

        }

        public async Task<UserProfileDto> GetProfile(string id)
        {
            var user = await _userManger.FindByIdAsync(id);

            if (user == null)
            {
                return new UserProfileDto()
                {
                    Message = "Invalid Email"
                };
            }
            var UserRole = await _userManger.GetRolesAsync(user);
            var profile = new UserProfileDto()
            {
                FirstName = user.FristName,
                LastName = user.LastName,
                userName = user.UserName,
                Address = user.Address,
                Email = user.Email,
                Phone = user.PhoneNumber,
                role = (List<string>)UserRole,
                CreatedAt = user.CreatedAt,

            };
            return profile;

        }


        public async Task<UpdateUserProfileDto> UpdateProfile(UpdateUserProfileDto updatedUser, string id)
        {
            var user = await _userManger.FindByIdAsync(id);

            if (user == null)
            {
                return new UpdateUserProfileDto();

            }

            if (await IsEmailNotRegistered(updatedUser.Email) == false && updatedUser.Email != user.Email)
            {

                return new UpdateUserProfileDto()
                {
                    Email = "EmailRegistered"
                };


            }

            if (await IsUserNameNotRegistered(updatedUser.userName) == false && updatedUser.userName != user.UserName)
            {
                return new UpdateUserProfileDto()
                {
                    userName = "UsernameRegisteredBefore_DSa12DF%RDAZXCVGR#$%734"
                };
            }


            user.FristName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.UserName = updatedUser.userName;
            user.Address = updatedUser.Address;
            user.Email = updatedUser.Email;
            user.PhoneNumber = updatedUser.Phone;

            await _userManger.UpdateAsync(user);

            return updatedUser;
        }

        public async Task<string> UpdatePassword(UpdatePasswordDto updatedpassword, ApplicationUser user)
        {
            //var passwordHasher = new PasswordHasher<ApplicationUser>();
            //var EncryptedPass = passwordHasher.HashPassword(user, updatedpassword.NewPassword);

            // Remove the old password
            await _userManger.RemovePasswordAsync(user);

            // Add the new password
            await _userManger.AddPasswordAsync(user, updatedpassword.NewPassword);
           

            // Note: You may not need to explicitly call UpdateAsync

            return "Ok";
        }

        public async Task<ApplicationUser> FindUserById(string id)
        {
            var user = await _userManger.FindByIdAsync(id);

            return user;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string Password)
        {
            if (!await _userManger.CheckPasswordAsync(user, Password))
                return false;
            return true;
        }

    }
}
