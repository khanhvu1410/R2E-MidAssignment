using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Username)
                .IsRequired();

            builder.Property(u => u.Password)
                .IsRequired();

            builder.Property(u => u.Role)
                .IsRequired();

            builder.HasData(
                new User { Id = 1, Username = "admin", Password = "123", Role = UserRole.SuperUser, Email = "admin123@gmail.com" },
                new User { Id = 2, Username = "user", Password = "123", Role = UserRole.NormalUser, Email = "user123@gmail.com" }
            );
        }
    }
}
