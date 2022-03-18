using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PasswordManager.Contexts;
using PasswordManager.DTOs;
using PasswordManager.Models;
using PasswordManager.Services.Interfaces;

namespace PasswordManager.Services
{
    public class StoredPasswordService : IStoredPasswordService
    {
        private readonly AppDbContext _context;
        public StoredPasswordService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddStoredPassword(AddStoredPasswordDTO addStoredPasswordDto)
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

            return storedPassword.Id;
        } 
        public async Task DeleteStoredPassword(int id)
        {
            var storedPassword = await _context.StoredPassword.FindAsync(id);
            if (storedPassword == null)
            {
                throw new ApiException($"Password: {id} could not be found.");
            }
            _context.StoredPassword.Remove(storedPassword);
            await _context.SaveChangesAsync();
        }

        public async Task<StoredPassword> GetStoredPassword(int id)
        {
            var storedPassword = await _context.StoredPassword.FindAsync(id);

            if (storedPassword == null)
            {
                throw new ApiException($"A Password: {id} could not be found.");
            }

            return storedPassword;
        }

        public async Task<List<StoredPassword>> GetStoredPasswords()
        {
            return await _context.StoredPassword.ToListAsync();
        }

        public async Task PutStoredPassword(int id, StoredPassword storedPassword)
        {
            if (id != storedPassword.Id)
            {
                throw new ApiException("StoredPassword and Id does not match.");
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
                    throw new ApiException($"A Password: {id} was found.");
                }
                else
                {
                    throw;
                }
            }
        }
        private bool StoredPasswordExists(int id)
        {
            return _context.StoredPassword.Any(StoredPassword => StoredPassword.Id == id);
        }
    }
}