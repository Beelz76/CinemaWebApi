using WebApi.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interfaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public UserController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegisterCredentials credentials)
        {
            if (!_userService.IsValidLogin(credentials.Login))
            {
                return BadRequest("Invalid name format");
            }

            if (await _userService.LoginExistsAsync(credentials.Login))
            {
                return Conflict("Login already exists");
            }

            var userUid = await _userService.RegisterAsync(credentials);

            return Ok(new JwtToken
            {
                Token = _jwtService.GenerateToken(userUid, credentials.Login, false)
            });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginCredentials credentials)
        {
            var userUid = await _userService.LoginAsync(credentials);
            
            if (userUid == Guid.Empty)
            {
                return BadRequest("Invalid login or password");
            }

            return Ok(new JwtToken
            {
                Token = _jwtService.GenerateToken(userUid, credentials.Login, await _userService.IsAdminAsync(userUid))
            });
        }

        [HttpGet]
        //[Authorize (Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            if (users.Count == 0)
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSingleUser(Guid userUid)
        {
            var user = await _userService.GetSingleUserAsync(userUid);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> GetUserInfo(Guid userUid)
        {
            var user = await _userService.GetUserInfoAsync(userUid);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdateUser(Guid userUid, UserUpdate userUpdate)
        {
            if (string.IsNullOrWhiteSpace(userUpdate.Login) || string.IsNullOrWhiteSpace(userUpdate.Password) ||
                string.IsNullOrWhiteSpace(userUpdate.FullName) || string.IsNullOrWhiteSpace(userUpdate.ConfirmedPassword))
            {
                return BadRequest("Wrong data");
            }

            if (await _userService.GetUserLoginAsync(userUid) != userUpdate.Login)
            {
                if (!_userService.IsValidLogin(userUpdate.Login))
                {
                    return BadRequest("Invalid name format");
                }

                if (await _userService.LoginExistsAsync(userUpdate.Login))
                {
                    return Conflict("Login already exists");
                }
            }

            if (!_userService.IsValidEmail(userUpdate.Email))
            {
                return BadRequest("Invalid email format");
            }

            if (userUpdate.Password != userUpdate.ConfirmedPassword)
            {
                return BadRequest("Failed to confirm password");
            }

            if (!await _userService.UpdateUserAsync(userUid, userUpdate))
            {
                return BadRequest("Failed to update user");
            }

            return Ok("User updated");
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserAdminStatus(Guid userUid)
        {
            if (!await _userService.UpdateUserAdminStatusAsync(userUid))
            {
                return BadRequest("Failed to update user status");
            }

            return Ok("User status updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(Guid userUid)
        {
            if (!await _userService.DeleteUserAsync(userUid))
            {
                return BadRequest("Failed to delete user");
            }

            return Ok("User deleted");
        }
    }
}