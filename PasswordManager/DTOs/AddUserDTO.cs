using System.ComponentModel.DataAnnotations;

namespace PasswordManager.DTOs
{
    public class AddUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string MasterPassword { get; set; }
    }
}
