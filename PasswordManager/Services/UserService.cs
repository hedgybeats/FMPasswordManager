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
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IDateTimeService _dateTimeService;
        public UserService(AppDbContext context, IDateTimeService dateTimeService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
        }

        public async Task<int> AddUser(AddUserDTO addUserDto)
        {

            var user = new User
            {
                Email = addUserDto.Email,
                MasterPassword = addUserDto.MasterPassword,
                CreatedAt = _dateTimeService.UtcNow()
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return user.Id;
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

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
