using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordManager.DTOs;
using PasswordManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PasswordManager.Services.Interfaces
{
    public interface IStoredPasswordService
    {
        Task<int> AddStoredPassword(AddStoredPasswordDTO addStoredPasswordDto);
        Task DeleteStoredPassword(int id);
        Task<StoredPassword> GetStoredPassword(int id);
        Task<List<StoredPassword>> GetStoredPasswords();
        Task PutStoredPassword(int id, StoredPassword storedPassword);
    }
}