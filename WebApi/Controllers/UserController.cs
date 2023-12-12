using WebApi.Contracts;
using WebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public UserController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        //[AllowAnonymous]
        public ActionResult<JwtToken> Register(UserRegisterCredentials credentials)
        {
            /*var regex = new Regex("/^[a-zA-Z][a-zA-Z0-9_]+$/");

            if (!regex.IsMatch(credentials.Login)) 
            {
                ModelState.AddModelError("login", "Invalid login format");

                return BadRequest(ModelState);
            }*/

            if (_userService.CheckLogin(credentials.Login))
            {
                ModelState.AddModelError("", "Login already exists");

                return BadRequest(ModelState);
            }

            var userUid = _userService.Register(credentials);

            return new JwtToken
            {
                Token = _jwtService.GenerateToken(userUid, credentials.Login)
            };
        }

        [HttpPost]
        //[AllowAnonymous]
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
                Token = _jwtService.GenerateToken(userUid.Value, credentials.Login)
            };
        }

        [HttpGet]
        //[Authorize]
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
        //[Authorize]
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
        //[Authorize]
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
        //[Authorize]
        public ActionResult UpdateUser(Guid userUid, UserUpdate userUpdate)
        {
            if (_userService.GetLogin(userUid) != userUpdate.Login)
            {
                if (_userService.CheckLogin(userUpdate.Login))
                {
                    ModelState.AddModelError("", "Login already exists");

                    return BadRequest(ModelState);
                }
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

        [HttpDelete]
        //[Authorize]
        public ActionResult DeleteUser(Guid userUid)
        {
            if (!_userService.DeleteUser(userUid))
            {
                return NotFound();
            }

            return Ok("User deleted");
        }
    }
}
