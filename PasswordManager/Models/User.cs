using PasswordManager.Models.Common;
using System.Collections.Generic;

namespace PasswordManager.Models
{
    public class User : BaseEntity
    {
        internal object id;

        public User()
        {
            StoredPasswords = new List<StoredPassword>();
        }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<StoredPassword> StoredPasswords { get; set; }
        public string ResetToken { get; set; }
    }
}
