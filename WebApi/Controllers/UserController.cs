using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Interface;

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
        public ActionResult<JwtToken> Register(UserRegisterCredentials credentials)
        {
            if (!_userService.CheckLoginRegex(credentials.Login))
            {
                ModelState.AddModelError("", "Invalid name format");

                return BadRequest(ModelState);
            }

            if (_userService.CheckLogin(credentials.Login))
            {
                ModelState.AddModelError("", "Login already exists");

                return BadRequest(ModelState);
            }

            var userUid = _userService.Register(credentials);

            return new JwtToken
            {
                Token = _jwtService.GenerateToken(userUid, credentials.Login, false)
            };
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult<JwtToken> Login(UserLoginCredentials credentials)
        {
            var userUid = _userService.Login(credentials);

            if (userUid == null)
            {
                ModelState.AddModelError("user", "Invalid login or password");

                return BadRequest(ModelState);
            }

            return new JwtToken
            {
                Token = _jwtService.GenerateToken(userUid.Value, credentials.Login, _userService.IsAdmin(userUid.Value))
            };
        }

        [HttpGet]
        //[Authorize (Roles = "Admin")]
        public ActionResult<List<User>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();

            if (users == null)
            {
                return NotFound("No users found");
            }

            return Ok(users);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public ActionResult<User> GetSingleUser(Guid userUid)
        {
            var user = _userService.GetSingleUser(userUid);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, User")]
        public ActionResult<UserInfo> GetUserInfo(Guid userUid)
        {
            var user = _userService.GetUserInfo(userUid);

            if (user == null)
            {
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin, User")]
        public ActionResult UpdateUser(Guid userUid, UserUpdate userUpdate)
        {
            if (userUpdate.Login == null || userUpdate.Password == null || 
                userUpdate.FullName == null || userUpdate.ConfirmedPassword == null)
            {
                return BadRequest();
            }

            if (_userService.GetLogin(userUid) != userUpdate.Login)
            {
                if (!_userService.CheckLoginRegex(userUpdate.Login))
                {
                    ModelState.AddModelError("", "Invalid name format");

                    return BadRequest(ModelState);
                }

                if (_userService.CheckLogin(userUpdate.Login))
                {
                    ModelState.AddModelError("", "Login already exists");

                    return BadRequest(ModelState);
                }
            }

            if (!_userService.CheckEmailRegex(userUpdate.Email))
            {
                ModelState.AddModelError("", "Invalid email format");

                return BadRequest(ModelState);
            }

            if (userUpdate.Password != userUpdate.ConfirmedPassword)
            {
                ModelState.AddModelError("", "Failed to confirm password");

                return BadRequest(ModelState);
            }

            if (!_userService.UpdateUser(userUid, userUpdate))
            {
                ModelState.AddModelError("", "Failed to update user");

                return BadRequest(ModelState);
            }

            return Ok("User updated");
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public ActionResult UpdateUserAdminStatus(Guid userUid)
        {
            if (!_userService.UpdateUserAdminStatus(userUid))
            {
                ModelState.AddModelError("", "Failed to update user status");

                return BadRequest(ModelState);
            }

            return Ok("User status updated");
        }

        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(Guid userUid)
        {
            if (!_userService.DeleteUser(userUid))
            {
                ModelState.AddModelError("", "Failed to delete user");

                return BadRequest(ModelState);
            }

            return Ok("User deleted");
        }
    }
}
