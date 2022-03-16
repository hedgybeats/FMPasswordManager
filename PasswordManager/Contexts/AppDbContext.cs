using Microsoft.EntityFrameworkCore;
using PasswordManager.Models;
using PasswordManager.Models.Common;
using PasswordManager.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace PasswordManager.Contexts
{
    public class AppDbContext : DbContext
    {
        private readonly IDateTimeService _dateTimeService;

        public AppDbContext(DbContextOptions<AppDbContext> options, IDateTimeService dateTimeService)
            : base(options)
        {
            _dateTimeService = dateTimeService;
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
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = _dateTimeService.UtcNow();
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTimeService.UtcNow();
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
