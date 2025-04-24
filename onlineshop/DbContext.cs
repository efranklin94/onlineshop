using Microsoft.EntityFrameworkCore;
using onlineshop.Helpers;
using onlineshop.Models;

namespace onlineshop;

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MyUser> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MyUser>().HasKey(x => x.Id);

        modelBuilder.Entity<MyUser>()
            .Property(x => x.Id)
            .UseIdentityColumn(seed: 1, increment: 2);

        modelBuilder.Entity<MyUser>().Property(x => x.FirstName).HasMaxLength(50);
        modelBuilder.Entity<MyUser>().Property(x => x.LastName).HasMaxLength(50);

        modelBuilder.Entity<MyUser>()
            .Property(x => x.PhoneNumber)
            .HasConversion(
                v => EncryptionHelper.Encrypt(v),
                v => EncryptionHelper.Decrypt(v)
            )
            .HasMaxLength(256);

        base.OnModelCreating(modelBuilder);
    }
}
