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

        public bool ValidatePassword(string input)
        {
            hasNumber = new Regex(@"[0-9]+");
            hasUpperChar = new Regex(@"[A-Z]+");
            hasMinimum8Chars = new Regex(@".{8,}");

            return (hasNumber.IsMatch(input) && hasUpperChar.IsMatch(input) && hasMinimum8Chars.IsMatch(input));
        }
    }
}
