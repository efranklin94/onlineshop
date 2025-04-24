using Microsoft.EntityFrameworkCore;
using onlineshop.Models;

namespace onlineshop;

public class MyDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<MyUser> Users { get; set; }

    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{
    //    modelBuilder.Entity<MyUser>(entity =>
    //    {
    //        entity.Property(e => e.FirstName).HasMaxLength(50);
    //        entity.Property(e => e.LastName).HasMaxLength(50);
    //        entity.Property(e => e.PhoneNumber).HasMaxLength(20);
    //    });
    //}
}
