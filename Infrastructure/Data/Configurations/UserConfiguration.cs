using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using onlineshop.Models;
using DomainModel.Models;
using onlineshop.Helpers;

namespace Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<MyUser>
{
    public void Configure(EntityTypeBuilder<MyUser> builder)
    {
        builder.UseTpcMappingStrategy();

        builder.HasKey(x => x.Id);

        builder.HasQueryFilter(x => !x.IsDeleted);

        //builder.Property(x => x.PhoneNumber).HasMaxLength(50);

        builder.Property(x => x.TrackingCode)
            .HasConversion(
            v => EncryptionHelper.Encrypt(v),
            v => EncryptionHelper.Decrypt(v)
        ).HasMaxLength(20);

        builder
            .HasMany(x => x.userOptions)
            .WithOne()
            .HasForeignKey("UserId").IsRequired()
            .OnDelete(DeleteBehavior.Cascade);


        builder.OwnsMany(x => x.userTags, tag =>
        {
            tag.WithOwner().HasForeignKey("UserId");
            tag.Property(x => x.Title).HasMaxLength(20);
            tag.Property(x => x.Priority).IsRequired();
            tag.HasKey("UserId", "Title", "Priority");
            tag.ToTable("UserTags");
        });

        builder.Property(x => x.CreatedBy).HasMaxLength(50);
        builder.Property(x => x.UpdatedBy).HasMaxLength(50);
        builder.Property(x => x.DeletedBy).HasMaxLength(50);
    }
}
public class UserOptionConfiguration : IEntityTypeConfiguration<UserOption>
{
    public void Configure(EntityTypeBuilder<UserOption> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description).HasMaxLength(100);
        builder.ToTable("UserOptions");
    }
}
