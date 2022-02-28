namespace PasswordManager.DTOs
{
    public class AddStoredPasswordDTO
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
    }
}
