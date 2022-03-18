﻿namespace PasswordManager.Services.Interfaces
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
        void ValidatePassword(string password);
    }
}
