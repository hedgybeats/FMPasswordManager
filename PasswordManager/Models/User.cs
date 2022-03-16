﻿using PasswordManager.Models.Common;
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
        public string MasterPassword { get; set; }
        public ICollection<StoredPassword> StoredPasswords { get; set; }
    }
}
