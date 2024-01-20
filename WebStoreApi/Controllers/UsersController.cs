using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Reposaitories.IReposaitories;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserReposaitory _userReposaitory;

        public UsersController(IUserReposaitory userReposaitory)
        {
            _userReposaitory = userReposaitory;
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(int? Page)
        {
            var users = await _userReposaitory.GetAllUsersAsync(Page);

            return Ok(users);
        }
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserByIdAsync(string Id)
        {
            var user = await _userReposaitory.GetUserByIdAsync(Id);
            if (user == null)
                return NotFound();

                return Ok(user);
        }
        [HttpGet("GetUserByUsername")]
        public async Task<IActionResult> GetUserByUserNameAsync(string Username)
        {
            var users = await _userReposaitory.GetUserByUserNameAsync(Username);
            if (users == null)
                return NotFound();

            return Ok(users);
        }
    }
}
