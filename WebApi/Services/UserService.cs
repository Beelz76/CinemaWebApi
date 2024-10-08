﻿using DatabaseAccessLayer;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using WebApi.Contracts;
using WebApi.Interfaces;

namespace WebApi.Services
{
    public class UserService : IUserService
    {
        private readonly CinemaDbContext _cinemaDbContext;

        public UserService(CinemaDbContext cinemaDbContext)
        {
            _cinemaDbContext = cinemaDbContext;
        }

        public async Task<Guid> RegisterAsync(RegisterUserDto credentials)
        {
            var user = new User
            {
                UserUid = Guid.NewGuid(),
                FullName = credentials.FullName,
                Login = credentials.Login,
                Password = GetHash(credentials.Password),
                IsAdmin = false,
            };

            await _cinemaDbContext.AddAsync(user);
            await _cinemaDbContext.SaveChangesAsync();

            return user.UserUid;
        }

        public async Task<Guid> LoginAsync(LoginUserDto dto)
        {
            var hashedPassword = GetHash(dto.Password);

            var user = await _cinemaDbContext.Set<User>().FirstOrDefaultAsync(x => x.Login == dto.Login && x.Password == hashedPassword);
            
            if (user == null) { return Guid.Empty; }

            return user.UserUid;
        }

        public async Task<IReadOnlyList<UserDto>> GetAllUsersAsync()
        {
            return await _cinemaDbContext.Set<User>()
                .Select(user => new UserDto
                {
                    UserUid = user.UserUid,
                    FullName = user.FullName,
                    Login = user.Login,
                    Password = GetHash(user.Password),
                    Email = user.Email,
                    IsAdmin = user.IsAdmin,
                })
                .ToListAsync();
        }

        public async Task<UserDto> GetSingleUserAsync(Guid userUid)
        {
            var user = await _cinemaDbContext.Set<User>()
                .Select(u => new UserDto
                {
                    UserUid = u.UserUid,
                    FullName = u.FullName,
                    Login = u.Login,
                    Email = u.Email,
                    Password = GetHash(u.Password),
                    IsAdmin = u.IsAdmin,
                })
                .FirstOrDefaultAsync(x => x.UserUid == userUid);

            return user;
        }

        public async Task<UserProfileDto> GetUserInfoAsync(Guid userUid)
        {
            var user = await _cinemaDbContext.Set<User>().FirstOrDefaultAsync(x => x.UserUid == userUid);

            if (user == null) { return null; }

            return new UserProfileDto
            {
                FullName = user.FullName,
                Login = user.Login,
                Password = user.Password,
                Email = user.Email
            };
        }

        public async Task<bool> UpdateUserAsync(Guid userUid, UpdateUserDto updateUserDto)
        {
            var totalRows = await _cinemaDbContext.Set<User>()
                .Where(x => x.UserUid == userUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.FullName, updateUserDto.FullName)
                    .SetProperty(p => p.Login, updateUserDto.Login)
                    .SetProperty(p => p.Password, GetHash(updateUserDto.Password))
                    .SetProperty(p => p.Email, updateUserDto.Email));
            
            return totalRows > 0;
        }

        public async Task<bool> UpdateUserAdminStatusAsync(Guid userUid)
        {
            var totalRows = await _cinemaDbContext.Set<User>()
                .Where(x => x.UserUid == userUid)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(p => p.IsAdmin, true));
            
            return totalRows > 0;
        }

        public async Task<bool> DeleteUserAsync(Guid userUid)
        {
            var totalRows = await _cinemaDbContext.Set<User>()
                .Where(x => x.UserUid == userUid)
                .ExecuteDeleteAsync();

            return totalRows > 0;
        }

        private static string GetHash(string password)
        {
            using var sha = SHA512.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes);
        }

        public async Task<bool> LoginExistsAsync(string login)
        {
            return await _cinemaDbContext.Set<User>().AnyAsync(x => x.Login == login);
        }

        public async Task<string> GetUserLoginAsync(Guid userUid)
        {
            var user = await _cinemaDbContext.Set<User>().FirstOrDefaultAsync(x => x.UserUid == userUid);

            if (user == null) { return string.Empty; }

            return user.Login;
        }

        public async Task<bool> IsAdminAsync(Guid userUid)
        {
            var user = await _cinemaDbContext.Set<User>().FirstOrDefaultAsync(x => x.UserUid == userUid);

            if (user == null) { return false; }

            return user.IsAdmin;
        }

        public async Task<bool> UserExistsAsync(Guid userUid)
        {
            return await _cinemaDbContext.Set<User>().AnyAsync(x => x.UserUid == userUid);
        }

        public bool IsValidLogin(string login)
        {
            return new Regex(@"^[a-zA-Z0-9][\w]{3,}$").IsMatch(login);
        }

        public bool IsValidEmail(string email)
        {
            return new Regex(@"^[\w-\.]+@([\w -]+\.)+[\w-]{2,4}$").IsMatch(email);
        }
    }
}