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

        // Only return the non-deleted items
        modelBuilder.Entity<MyUser>().HasQueryFilter(x => EF.Property<bool>(x, "IsDeleted") == false);

        modelBuilder.Entity<MyUser>().Property(x => x.TrackingCode).HasMaxLength(10);

        modelBuilder.Entity<MyUser>().HasIndex(u => u.Email).IsUnique().HasDatabaseName("IX_MyUser_Email");


        base.OnModelCreating(modelBuilder);
        #endregion

        #region City & Country
        modelBuilder.Entity<City>().HasKey(x => x.Id);
        modelBuilder.Entity<City>().Property(x => x.Name).HasMaxLength(50);

        modelBuilder.Entity<City>().HasOne(city => city.Country).WithMany(country => country.Cities).HasForeignKey(city => city.CountryId);

        modelBuilder.Entity<Country>().HasKey(x => x.Id);
        modelBuilder.Entity<Country>().Property(x => x.Name).HasMaxLength(50);

        modelBuilder.Entity<Country>().HasData(
            new Country { Id = 1, Name = "Iran" },
            new Country { Id = 2, Name = "USA" },
            new Country { Id = 3, Name = "France" }
        );

        modelBuilder.Entity<City>().HasData([
            new City{ Id = 1, Name = "Tehran", CountryId = 1 , Type = Enums.CitiesType.Capital},
            new City{ Id = 2, Name = "Tabriz", CountryId = 1 , Type = Enums.CitiesType.Metropolis},
            new City{ Id = 3, Name = "Semnan", CountryId = 1 , Type = Enums.CitiesType.SmallCity},
            new City{ Id = 4, Name = "New York", CountryId = 2 , Type = Enums.CitiesType.Metropolis},
            new City{ Id = 5, Name = "Paris", CountryId = 3 , Type = Enums.CitiesType.Metropolis},
        ]);
        #endregion
    }
}
