using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    internal class BorrowingRequestConfiguration : IEntityTypeConfiguration<BookBorrowingRequest>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequest> builder)
        {
            builder.Property(bbr => bbr.RequestorId)
                .IsRequired();

            builder.Property(bbr => bbr.RequestedDate)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(bbr => bbr.Status)
                .IsRequired();

            builder.HasOne(bbr => bbr.Requestor)
                .WithMany(r => r.BookBorrowingRequests)
                .HasForeignKey(bbr => bbr.RequestorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bbr => bbr.Approver)
                .WithMany()
                .HasForeignKey(bbr => bbr.ApproverId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
