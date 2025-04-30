using System.Reflection;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Persistence.DataSeeders;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence
{
    public class LibraryManagementDbContext : DbContext
    {
        public LibraryManagementDbContext(DbContextOptions<LibraryManagementDbContext> options) 
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // Seed data
            CategorySeeder.Seed(modelBuilder);
            BookSeeder.Seed(modelBuilder);
        }

        public DbSet<User> Users {  get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<BookBorrowingRequest> BookBorrowingRequests { get; set; }

        public DbSet<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
    }
}
