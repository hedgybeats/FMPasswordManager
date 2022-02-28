using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoredPasswordController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoredPasswordController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/StoredPassword
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StoredPassword>>> GetStoredPassword()
        {
            return Ok(await _context.StoredPassword.ToListAsync());
        }

        // GET: api/StoredPassword/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StoredPassword>> GetStoredPassword(int id)
        {
            var storedPassword = await _context.StoredPassword.FindAsync(id);

            if (storedPassword == null)
            {
                throw new ApiException($"A stored password with id {id} could not be found.");
            }

            return Ok(storedPassword);
        }

        // PUT: api/StoredPassword/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStoredPassword(int id, StoredPassword storedPassword)
        {
            if (id != storedPassword.Id)
            {
                return BadRequest();
            }

            _context.Entry(storedPassword).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoredPasswordExists(id))
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

        // POST: api/StoredPassword
        [HttpPost]
        public async Task<ActionResult<StoredPassword>> PostStoredPassword(AddStoredPasswordDTO addStoredPasswordDto)
        {
            var user = await _context.User.Where(x => x.Id == addStoredPasswordDto.UserId)
                                    .Select(x => new { x.Id })
                                    .SingleOrDefaultAsync();

            if (user == null) throw new ApiException($"User with id {addStoredPasswordDto.UserId} could not be found.");

            var storedPassword = new StoredPassword
            {
                Name = addStoredPasswordDto.Name,
                Link = addStoredPasswordDto.Link,
                Password = addStoredPasswordDto.Password,
                UserId = user.Id
            };

            _context.StoredPassword.Add(storedPassword);
            await _context.SaveChangesAsync();

            return Ok(new { id = storedPassword.Id });
        }

        // DELETE: api/StoredPassword/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStoredPassword(int id)
        {
            var storedPassword = await _context.StoredPassword.FindAsync(id);
            if (storedPassword == null)
            {
                throw new ApiException($"A stored password with id {id} could not be found.");
            }

            _context.StoredPassword.Remove(storedPassword);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool StoredPasswordExists(int id)
        {
            return _context.StoredPassword.Any(e => e.Id == id);
        }
    }
}
