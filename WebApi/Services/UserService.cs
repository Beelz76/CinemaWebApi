using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace WebApi.Services
{
    public class UserService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public UserService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public Guid Register(Contracts.UserRegisterCredentials credentials)
        {
            var user = new User
            {
                UserUid = Guid.NewGuid(),
                FullName = credentials.FullName,
                Login = credentials.Login,
                Password = GetHash(credentials.Password),
                IsAdmin = false,
            };

            _cinemaDbContext.Add(user);
            _cinemaDbContext.SaveChanges();

            return user.UserUid;
        }

        public Guid? Login(Contracts.UserLoginCredentials credentials)
        {
            var hashedPassword = GetHash(credentials.Password);

            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.Login == credentials.Login && x.Password == hashedPassword);

            return user?.UserUid;
        }

        public List<Contracts.User>? GetAllUsers()
        {
            var users = _cinemaDbContext.Set<User>().ToList();

            if (users.Count == 0) { return null; }

            return users.Select(user => new Contracts.User
            {
                UserUid = user.UserUid,
                FullName = user.FullName,
                Login = user.Login,
                Password = GetHash(user.Password),
                Email = user.Email,
                IsAdmin = user.IsAdmin,
            }).ToList();
        }

        [HttpGet]
        public Contracts.User? GetSingleUser(Guid userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return new Contracts.User
            {
                UserUid = user.UserUid,
                FullName = user.FullName,
                Login = user.Login,
                Email = user.Email,
                Password = GetHash(user.Password),
                IsAdmin = user.IsAdmin,
            };
        }

        public Contracts.UserInfo? GetUserInfo(Guid userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return new Contracts.UserInfo
            {
                FullName = user.FullName,
                Login = user.Login,
                Password = user.Password,
                Email = user.Email
            };
        }

        public bool UpdateUser(Guid userUid, Contracts.UserUpdate userUpdate)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            user.FullName = userUpdate.FullName;
            user.Login = userUpdate.Login;
            user.Password = GetHash(userUpdate.Password);
            user.Email = userUpdate.Email;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool DeleteUser(Guid userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            _cinemaDbContext.Remove(user);

            return _cinemaDbContext.SaveChanges() > 0;
        }

        private string GetHash(string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(bytes);
        }

        public bool CheckLogin(string login)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.Login == login);

            if (user == null) { return false; }

            return true;
        }

        public string? GetLogin(Guid userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return user.Login;
        }

        public bool IsAdmin(Guid userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            if (user.IsAdmin == true)
            {
                return true;
            }

            return false;
        }

        public bool IsUserExists(Guid? userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; }

            return true;
        }

        public bool UpdateUserAdminStatus(Guid userUid)
        {
            var user = _cinemaDbContext.Set<User>().SingleOrDefault(x => x.UserUid == userUid);

            if (user == null) { return false; };

            user.IsAdmin = true;

            return _cinemaDbContext.SaveChanges() > 0;
        }

        public bool CheckRegex(string name)
        {
            var regex = new Regex(@"^[a-zA-Z][\w]{3,}$");

            if (!regex.IsMatch(name))
            {
                return false;
            }

            return true;
        }

        public bool CheckEmailRegex(string email)
        {
            var regex = new Regex(@"^[\w-\.]+@([\w -]+\.)+[\w-]{2,4}$");

            if (!regex.IsMatch(email))
            {
                return false;
            }

            return true;
        }
    }
}
