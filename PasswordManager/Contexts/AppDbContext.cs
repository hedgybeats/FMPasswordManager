using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;

namespace PasswordManager.Contexts
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = null!;
        public DbSet<StoredPassword> StoredPassword { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StoredPassword>()
                        .HasOne(x => x.User)
                        .WithMany(x => x.StoredPasswords)
                        .HasForeignKey(x => x.UserId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
