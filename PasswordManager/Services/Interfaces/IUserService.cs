using PasswordManager.DTOs;
using PasswordManager.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PasswordManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> AddUser(AddUserDTO addUserDto);
        Task<int> AuthenticateUser(AuthenticateUserDTO authenticateUserDto);
        Task DeleteUser(int id);
        Task<User> GetUser(int id);
        Task<List<User>> GetUsers();
        Task PutUser(int id, User user);
        string GenerateJwtToken(int userId);
        Task<bool> EmailExists(string email);
    }
}
