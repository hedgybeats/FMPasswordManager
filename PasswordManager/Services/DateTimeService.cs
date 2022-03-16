using PasswordManager.Services.Interfaces;
using System;

namespace PasswordManager.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeService()
        {

        }
        public DateTime UtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}