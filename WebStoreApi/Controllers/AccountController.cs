using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;
using WebStoreApi.Services;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAccountReposaitory _accountReposaitory;

        public AccountController(IConfiguration configuration, IAccountReposaitory accountReposaitory, ILogger<AccountController> logger)
        {
            _configuration = configuration;
            _accountReposaitory = accountReposaitory;
            _logger = logger;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto user)
        {
            if (await _accountReposaitory.IsEmailNotRegistered(user.Email) == false)
            {
                ModelState.AddModelError("Email", "Email is already Registered");
                return BadRequest(ModelState);
            }

            if (await _accountReposaitory.IsUserNameNotRegistered(user.userName) == false)
            {
                ModelState.AddModelError("Username", "Username is already taken");
                return BadRequest(ModelState);
            }
            var RegisteredUser = await _accountReposaitory.Register(user);
            return Ok(RegisteredUser);

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, [FromBody] string password)
        {
            var result = await _accountReposaitory.Login(email, password);
            if (result.profile == null)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            var id = JwtReader.GetUserId(User);
            if (id == null)
                return Unauthorized();

            var profile = await _accountReposaitory.GetProfile(id);

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileDto user)
        {
            var id = JwtReader.GetUserId(User);
            if (id == null)
                return Unauthorized();

            var profile = await _accountReposaitory.UpdateProfile(user, id);
            if (profile.Email == "EmailRegistered")
            {
                ModelState.AddModelError("Email", "Email is already Registered");
                return BadRequest(ModelState);
            }

            if (profile.userName == "UsernameRegisteredBefore_DSa12DF%RDAZXCVGR#$%734")
            {
                ModelState.AddModelError("Username", "Username is already Registered");
                return BadRequest(ModelState);
            }

            return Ok(profile);
        }

        [Authorize]
        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordDto updatePasswordDto)
        {
            var id = JwtReader.GetUserId(User);
            if (id == null)
                return Unauthorized();

            var user = await _accountReposaitory.FindUserById(id);
            if (user == null)
                return BadRequest("Something went wrong");

            if (await _accountReposaitory.CheckPasswordAsync(user, updatePasswordDto.OldPassword) == false)
            {
                ModelState.AddModelError("OldPassword", "Incorrect Password");
                return BadRequest(ModelState);
            }

             await _accountReposaitory.UpdatePassword(updatePasswordDto, user);


            return Ok();
        }
    }
}
