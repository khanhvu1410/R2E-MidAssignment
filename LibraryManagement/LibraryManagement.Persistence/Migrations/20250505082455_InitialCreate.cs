using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: false),
                    PublicationYear = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestorId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<int>(type: "int", nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequests_Users_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequests_Users_RequestorId",
                        column: x => x.RequestorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BookBorrowingRequestDetails",
                columns: table => new
                {
                    BookBorrowingRequestId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookBorrowingRequestDetails", x => new { x.BookBorrowingRequestId, x.BookId });
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetails_BookBorrowingRequests_BookBorrowingRequestId",
                        column: x => x.BookBorrowingRequestId,
                        principalTable: "BookBorrowingRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookBorrowingRequestDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Fiction" },
                    { 2, "Science" },
                    { 3, "History" },
                    { 4, "Biography" },
                    { 5, "Children" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "PublicationYear", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, "Harper Lee", 1, "A classic novel set in the American South, dealing with racial injustice and moral growth through the eyes of young Scout Finch.", "9780061120084", 1960, 10, "To Kill a Mockingbird" },
                    { 2, "George Orwell", 1, "A dystopian masterpiece exploring totalitarianism, surveillance, and the manipulation of truth in a futuristic society.", "9780451524935", 1949, 10, "1984" },
                    { 3, "Stephen Hawking", 2, "A groundbreaking exploration of cosmology, black holes, and the origins of the universe for general readers.", "9780553109535", 1988, 10, "A Brief History of Time" },
                    { 4, "Richard Dawkins", 2, "Introduces the concept of genes as the primary drivers of evolution, shaping behavior and survival strategies.", "9780192860927", 1976, 10, "The Selfish Gene" },
                    { 5, "Jared Diamond", 3, "Examines geographic and environmental factors that shaped the dominance of certain civilizations throughout history.", "9780393317558", 1997, 10, "Guns, Germs, and Steel" },
                    { 6, "David McCullough", 3, "A detailed biography of Orville and Wilbur Wright, chronicling their journey to inventing the first successful airplane.", "9781476728742", 2015, 10, "The Wright Brothers" },
                    { 7, "Anne Frank", 4, "The poignant and powerful diary of a Jewish girl hiding from the Nazis during World War II.", "9780553296983", 1947, 10, "The Diary of a Young Girl" },
                    { 8, "Walter Isaacson", 4, "The authorized biography of Apple's visionary co-founder, revealing his genius, creativity, and complex personality.", "9781451648539", 2011, 10, "Steve Jobs" },
                    { 9, "E.B. White", 5, "A heartwarming tale of friendship between a pig named Wilbur and a clever spider named Charlotte.", "9780061124952", 1952, 10, "Charlotte's Web" },
                    { 10, "Dr. Seuss", 5, "A whimsical and rhyming story about a mischievous cat who turns a rainy day into an unforgettable adventure.", "9780394800011", 1957, 10, "The Cat in the Hat" },
                    { 11, "F. Scott Fitzgerald", 1, "A portrait of the Jazz Age through the tragic story of the mysterious millionaire Jay Gatsby and his love for Daisy Buchanan.", "9780743273565", 1925, 8, "The Great Gatsby" },
                    { 12, "Frank Herbert", 1, "A science fiction epic about politics, religion, and power on the desert planet Arrakis, home to the valuable spice melange.", "9780441172719", 1965, 7, "Dune" },
                    { 13, "Carl Sagan", 2, "A journey through the universe exploring astronomy, biology, and the history of science, connecting humanity to the cosmos.", "9780345539434", 1980, 6, "Cosmos" },
                    { 14, "Siddhartha Mukherjee", 2, "A comprehensive history of genetics, from its discovery to modern breakthroughs and ethical dilemmas.", "9781476733500", 2016, 5, "The Gene: An Intimate History" },
                    { 15, "Yuval Noah Harari", 3, "Explores the history of Homo sapiens from the Stone Age to the 21st century, examining key revolutions in human development.", "9780062316097", 2011, 9, "Sapiens: A Brief History of Humankind" },
                    { 16, "Peter Frankopan", 3, "Reinterprets world history with Asia and the Silk Road trade routes at the center of global exchange and power.", "9781408839997", 2015, 4, "The Silk Roads: A New History of the World" },
                    { 17, "Michelle Obama", 4, "An intimate memoir by the former First Lady, chronicering her journey from Chicago's South Side to the White House.", "9781524763138", 2018, 7, "Becoming" },
                    { 18, "Tara Westover", 4, "A memoir about a woman who leaves her survivalist family and goes on to earn a PhD from Cambridge University.", "9780399590504", 2018, 6, "Educated" },
                    { 19, "J.K. Rowling", 5, "The first book in the Harry Potter series, following a young boy's discovery of his magical heritage at Hogwarts School.", "9780590353427", 1997, 10, "Harry Potter and the Sorcerer's Stone" },
                    { 20, "Maurice Sendak", 5, "A classic picture book about Max's imaginative journey to the land of the Wild Things, where he becomes king.", "9780060254926", 1963, 8, "Where the Wild Things Are" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequestDetails_BookId",
                table: "BookBorrowingRequestDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequests_ApproverId",
                table: "BookBorrowingRequests",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_BookBorrowingRequests_RequestorId",
                table: "BookBorrowingRequests",
                column: "RequestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookBorrowingRequestDetails");

            migrationBuilder.DropTable(
                name: "BookBorrowingRequests");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
