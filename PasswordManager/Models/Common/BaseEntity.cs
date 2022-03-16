using System;

namespace PasswordManager.Models.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public int Id { get; set; }

    }
}
