using Microsoft.EntityFrameworkCore;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IPasswordService _passwordService;

        public UserService(AppDbContext context, IDateTimeService dateTimeService, IPasswordService passwordService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _passwordService = passwordService;
        }

        public async Task<int> AddUser(AddUserDTO addUserDto)
        {
            if (!_passwordService.ValidatePassword(addUserDto.MasterPassword))
                throw new ApiException("Password does not meet security conditions.");

            var user = new User
            {
                Email = addUserDto.Email,
                MasterPassword = addUserDto.MasterPassword,
                HashedPassword = _passwordService.HashPassword(addUserDto.MasterPassword)
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<int> AuthenticateUser(AuthenticateUserDTO authenticateUserDto)
        {
            var user = await _context.User.Where(x => x.Email.Equals(authenticateUserDto.UserName)).SingleOrDefaultAsync();

            if (user == null || !_passwordService.VerifyPassword(authenticateUserDto.MasterPassword, user.HashedPassword))
                throw new ApiException("Username or password is incorrect.");
            //will return jwt
            return user.Id;
        }
    }
}
