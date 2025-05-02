using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedMoreBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "Description", "ISBN", "PublicationYear", "Quantity", "Title" },
                values: new object[,]
                {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}
