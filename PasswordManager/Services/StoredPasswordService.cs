using System;
using System.Threading.Tasks;
using PasswordManager.Services.Interfaces;

namespace PasswordManager.Services
{
    public class StoredPasswordService : IStoredPasswordService
    {
        public StoredPasswordService()
        {

        }

        public Task<string> GetPasswordAsync(string password)
        {
            throw new NotImplementedException();
        }

        public StoredPasswordService UtcNow()
        {
            throw new NotImplementedException();
        }
    }
}