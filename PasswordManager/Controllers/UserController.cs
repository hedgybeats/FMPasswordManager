using EmailSender.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDateTimeService _dateTimeService;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IStoredPasswordService _storedPasswordService;
        public UserController(AppDbContext context, IDateTimeService dateTimeService, IEmailSenderService emailSenderService, IStoredPasswordService storedPassword)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _emailSenderService = emailSenderService;
            _storedPasswordService = storedPassword;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return Ok(await _context.User.Include(x => x.StoredPasswords).ToListAsync());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.Include(x => x.StoredPasswords).SingleOrDefaultAsync(x => x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(AddUserDTO addUserDto)
        {
            var user = new User
            {
                Email = addUserDto.Email,
                MasterPassword = addUserDto.MasterPassword,
                CreatedAt = _dateTimeService.UtcNow()
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { id = user.Id });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetMasterPassword(string email)
        {
            if (!await EmailExists(email))
                {
                    throw new ApiException($"Email adress {email} could not be found");
                }
            await _emailSenderService.SenderEmailAsync(email, "Reset Your Password", "reset password body");
            return Ok();

        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                throw new ApiException($"A user with id {id} could not be found.");
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private async Task<bool> UserExists(int id)
        {
            return await _context.User.AnyAsync(user => user.Id == id);
        }
        private async Task<bool> EmailExists(string email)
        {
         return await _context.User.AnyAsync(user => user.Email == email);
        }
    }
}
