using PasswordManager.Models.Common;
using System.Collections.Generic;

namespace PasswordManager.Models
{
    public class User : BaseEntity
    {
        public User()
        {
            StoredPasswords = new List<StoredPassword>();
        }
        public string Email { get; set; }
        public string MasterPassword { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<StoredPassword> StoredPasswords { get; set; }
    }
}
