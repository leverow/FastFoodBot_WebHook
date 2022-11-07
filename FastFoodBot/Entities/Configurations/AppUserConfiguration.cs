using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastFoodBot.Entities.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id).ValueGeneratedOnAdd().IsRequired();
        builder.Property(b => b.UserId);
        builder.Property(b => b.FirstName).HasColumnType("nvarchar(25)");
        builder.Property(b => b.LastName).HasColumnType("nvarchar(25)");
        builder.Property(b => b.Username).HasColumnType("nvarchar(25)");
        builder.Property(b => b.LanguageCode).HasColumnType("nvarchar(10)");
        builder.Property(b => b.PhoneNumber).HasColumnType("nvarchar(25)");
    }
}