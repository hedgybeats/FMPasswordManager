using EmailSender.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
using System.Collections.Generic;
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
        private readonly IUserService _userService;
        public UserController(AppDbContext context, IDateTimeService dateTimeService, IEmailSenderService emailSenderService, IUserService userService)
        {
            _context = context;
            _dateTimeService = dateTimeService;
            _emailSenderService = emailSenderService;
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<ICollection<User>>> GetUsers()
        {
            return await _userService.GetUsers();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            return await _userService.GetUser(id);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            await _userService.PutUser(id, user);
            return Ok();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(AddUserDTO addUserDto)
        {
            return Ok(new { id = await _userService.AddUser(addUserDto) });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetMasterPassword(string email)
        {
            await _userService.ResetMasterPassword(email);
            return Ok();
        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdateMasterPassword(UpdateMasterPasswordDTO updateMasterPasswordDTO)
        {
            await _userService.UpdateMasterPassword(updateMasterPasswordDTO);
            return Ok();
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthentiateUser(AuthenticateUserDTO authenticateUserDto)
        {
            return Ok(new { id = await _userService.AuthenticateUser(authenticateUserDto) });
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }
    }
}
