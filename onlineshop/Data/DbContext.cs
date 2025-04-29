using Microsoft.EntityFrameworkCore;
using onlineshop.Helpers;
using onlineshop.Models;

namespace onlineshop.Data;

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MyUser> Users { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region User 
        modelBuilder.HasSequence<int>("UserSequence", "dbo")
            .IncrementsBy(2);

        modelBuilder.Entity<MyUser>().HasKey(x => x.Id);

        modelBuilder.Entity<MyUser>()
            .Property(x => x.Id)
            .HasDefaultValueSql("NEXT VALUE FOR dbo.UserSequence");

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
        #endregion

        #region City
        modelBuilder.Entity<City>().HasKey(x => x.Id);
        modelBuilder.Entity<City>().Property(x => x.Name).HasMaxLength(50);
        modelBuilder.Entity<City>().Property(x => x.Country).HasMaxLength(50);

        modelBuilder.Entity<City>().HasData([
            new City{ Id = 1, Name = "Tehran", Country = "Iran" },
            new City{ Id = 2, Name = "Tabriz", Country = "Iran"  },
            new City{ Id = 3, Name = "Semnan", Country = "Iran"  },
            new City{ Id = 4, Name = "New York", Country = "USA"  },
            new City{ Id = 5, Name = "Paris", Country = "France"  },
        ]);
        #endregion
    }
}
