using EmailSender.Services.Interfaces;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
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
    }
}
