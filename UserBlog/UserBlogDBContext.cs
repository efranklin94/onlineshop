using Microsoft.EntityFrameworkCore;

namespace UserBlog;

public class UserBlogDBContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(c =>
        {
            c.HasKey(x => x.Id);
            c.Property(x => x.Id).ValueGeneratedNever();
            c.Property(x => x.Title).HasMaxLength(100);
            c.Property(x => x.Gearbox).HasMaxLength(10);
        });

        base.OnModelCreating(modelBuilder);
    }
}
