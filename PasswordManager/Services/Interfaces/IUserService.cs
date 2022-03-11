using PasswordManager.DTOs;
using System.Threading.Tasks;

namespace PasswordManager.Services.Interfaces
{
    public interface IUserService
    {
        Task<int> AddUser(AddUserDTO addUserDto);
    }
}
