using PasswordManager.Models.Common;

namespace PasswordManager.Models
{
    public class StoredPassword : BaseEntity
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Link { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
