namespace PasswordManager.Services.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool ComparePassword(bool password);
    }
}
