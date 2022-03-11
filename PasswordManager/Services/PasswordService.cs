using PasswordManager.Services.Interfaces;

namespace PasswordManager.Services
{
    public class PasswordService : IPasswordService
    {
        public bool ComparePassword(bool password)
        {
            throw new System.NotImplementedException();
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
