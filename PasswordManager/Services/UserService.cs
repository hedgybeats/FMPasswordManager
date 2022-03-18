using EmailSender.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IPasswordService _passwordService;
        private readonly IEmailSenderService _emailSenderService;

        public UserService(AppDbContext context, IDateTimeService dateTimeService, IPasswordService passwordService, IEmailSenderService emailSenderService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _passwordService = passwordService;
            _emailSenderService = emailSenderService;
        }

        public async Task<int> AddUser(AddUserDTO addUserDto)
        {
            _passwordService.ValidatePassword(addUserDto.MasterPassword);

            var user = new User
            {
                Email = addUserDto.Email,
                HashedPassword = _passwordService.HashPassword(addUserDto.MasterPassword)
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task<string> AuthenticateUser(AuthenticateUserDTO authenticateUserDto)
        {
            var user = await _context.User.Where(x => x.Email.Equals(authenticateUserDto.Email)).SingleOrDefaultAsync();

            if (user == null || !_passwordService.VerifyPassword(authenticateUserDto.MasterPassword, user.HashedPassword))
                throw new ApiException("Username or password is incorrect.");
            return GenerateJwtToken(user.Id);
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                throw new ApiException($"A user with id {id} could not be found.");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.User.Include(x => x.StoredPasswords).SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                throw new ApiException($"A user with id {id} could not be found.");
            }

            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.User.Include(x => x.StoredPasswords).ToListAsync();
        }

        public async Task PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                throw new ApiException("User and Id does not match.");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    throw new ApiException($"A user with id {id} could not be found.");
                }
                else
                {
                    throw;
                }
            }
        }

        public string GenerateJwtToken(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", userId.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task ResetMasterPassword(string email)
        {
            var user = await _context.User.Where(x => x.Email.Equals(email)).SingleOrDefaultAsync();

            if (user == null) throw new ApiException($"A user with email {email} could not be found.");

            user.ResetToken = RandomToken();
            await _context.SaveChangesAsync();
            await _emailSenderService.SenderEmailAsync(email, "Reset Password", $"Your reset password token is {user.ResetToken}");
        }

        public async Task UpdateMasterPassword(UpdateMasterPasswordDTO updateMasterPasswordDTO)
        {
            var user = await _context.User.Where(user => user.ResetToken.Equals(updateMasterPasswordDTO.Token)).SingleOrDefaultAsync();

            if (user == null) throw new ApiException("A user with this reset token could not be found");

            _passwordService.ValidatePassword(updateMasterPasswordDTO.NewPassword);

            user.HashedPassword = _passwordService.HashPassword(updateMasterPasswordDTO.NewPassword);

            await _context.SaveChangesAsync();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        public async Task<bool> EmailExists(string email)
        {
            return await _context.User.AnyAsync(user => user.Email == email);
        }

        private string RandomToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var uintBuffer = new byte[40];
            rng.GetBytes(uintBuffer);
            return BitConverter.ToString(uintBuffer, 0).Replace("-", "");
        }
    }
}
