using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoredPasswordController : ControllerBase
    {
        private readonly IStoredPasswordService _storedPasswordService;

        public StoredPasswordController(IStoredPasswordService storedPasswordService)
        {
            _storedPasswordService = storedPasswordService;
        }

        // GET: api/StoredPassword
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<StoredPassword>>> GetStoredPassword()
        {
            return await _storedPasswordService.GetStoredPasswords();
        }

        // GET: api/StoredPassword/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<StoredPassword>> GetStoredPassword(int id)
        {
            return await _storedPasswordService.GetStoredPassword(id);
        }

        // PUT: api/StoredPassword/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutStoredPassword(int id, StoredPassword storedPassword)
        {
            await _storedPasswordService.PutStoredPassword(id, storedPassword);
            return Ok();
        }

        // POST: api/StoredPassword
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<StoredPassword>> PostStoredPassword(AddStoredPasswordDTO addStoredPasswordDto)
        {
            return Ok(new { id = await _storedPasswordService.AddStoredPassword(addStoredPasswordDto) });
        }

        // DELETE: api/StoredPassword/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteStoredPassword(int id)
        {
            await _storedPasswordService.DeleteStoredPassword(id);
            return Ok();
        }

        [Authorize]
        private bool StoredPasswordExists(int id)
        {
            return _context.StoredPassword.Any(e => e.Id == id);
        }
    }
}
