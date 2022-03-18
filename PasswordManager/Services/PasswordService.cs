using PasswordManager.Models;
using PasswordManager.Services.Interfaces;
using System.Text.RegularExpressions;

namespace PasswordManager.Services
{
    public class PasswordService : IPasswordService
    {
        private Regex hasNumber, hasUpperChar, hasMinimum8Chars;

        public bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public void ValidatePassword(string password)
        {
            hasNumber = new Regex(@"[0-9]+");
            hasUpperChar = new Regex(@"[A-Z]+");
            hasMinimum8Chars = new Regex(@".{8,}");

            if (hasNumber.IsMatch(password) && hasUpperChar.IsMatch(password) && hasMinimum8Chars.IsMatch(password))
                throw new ApiException("Password does not meet security conditions.");

        }
    }
}
