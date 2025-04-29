using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Persistence.Configurations
{
    internal class BookBorrowingRequestDetailsConfiguration : IEntityTypeConfiguration<BookBorrowingRequestDetails>
    {
        public void Configure(EntityTypeBuilder<BookBorrowingRequestDetails> builder)
        {
            builder.HasKey(bbrd => new { bbrd.BookBorrowingRequestId, bbrd.BookId });

            builder.Property(bbrd => bbrd.BookBorrowingRequestId)
                .IsRequired();

            builder.Property(bbrd => bbrd.BookId)
                .IsRequired();

            builder.HasOne(bbrd => bbrd.BookBorrowingRequest)
                .WithMany(bbr => bbr.BookBorrowingRequestDetails)
                .HasForeignKey(bbrd => bbrd.BookBorrowingRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(bbrd => bbrd.Book)
                .WithMany(b => b.BookBorrowingRequestDetails)
                .HasForeignKey(bbrd => bbrd.BookId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
