using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories;
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
        private readonly IMailingService _mailingService;

        public AccountController(IConfiguration configuration, IAccountReposaitory accountReposaitory, ILogger<AccountController> logger, IMailingService mailingService)
        {
            _configuration = configuration;
            _accountReposaitory = accountReposaitory;
            _logger = logger;
            _mailingService = mailingService;
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

        [HttpPost("ForgotPassword")]
        
        public async Task<IActionResult> ForgotPaswword(string Email)
        {
            var user = await _accountReposaitory.FindUserByEmailAsync(Email);
            if (user == null)
                return NotFound();

            var token = await _accountReposaitory.GeneratePasswordToken(user);
            
            string Body = "Dear " + user.FristName + " " + user.LastName + "\n" +
               "We have received your request to reset your password. \n" +
               "please copy the token and paste it in reset password section .       \n" +
               token;

            await _mailingService.SendEmailAsync(user.Email, "Resest Password", Body);

            return Ok();

        }



        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            var user = await _accountReposaitory.FindUserByEmailAsync(resetPassword.Email);
            if (user == null)
                return NotFound();

            await _accountReposaitory.ChangePasswordAsync(user, resetPassword.Token, resetPassword.newPassword);

            return Ok();

        }

    }
}
