using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Persistence.DataSeeders
{
    internal static class BookSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                // Fiction books
                new Book
                {
                    Id = 1,
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "9780061120084",
                    PublicationYear = 1960,
                    Quantity = 10,
                    CategoryId = 1,
                    Description = "A classic novel set in the American South, dealing with racial injustice and moral growth through the eyes of young Scout Finch."
                },
                new Book
                {
                    Id = 2,
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "9780451524935",
                    PublicationYear = 1949,
                    Quantity = 10,
                    CategoryId = 1,
                    Description = "A dystopian masterpiece exploring totalitarianism, surveillance, and the manipulation of truth in a futuristic society."
                },

                // Science books
                new Book
                {
                    Id = 3,
                    Title = "A Brief History of Time",
                    Author = "Stephen Hawking",
                    ISBN = "9780553109535",
                    PublicationYear = 1988,
                    Quantity = 10,
                    CategoryId = 2,
                    Description = "A groundbreaking exploration of cosmology, black holes, and the origins of the universe for general readers."
                },
                new Book
                {
                    Id = 4,
                    Title = "The Selfish Gene",
                    Author = "Richard Dawkins",
                    ISBN = "9780192860927",
                    PublicationYear = 1976,
                    Quantity = 10,
                    CategoryId = 2,
                    Description = "Introduces the concept of genes as the primary drivers of evolution, shaping behavior and survival strategies."
                },

                // History books
                new Book
                {
                    Id = 5,
                    Title = "Guns, Germs, and Steel",
                    Author = "Jared Diamond",
                    ISBN = "9780393317558",
                    PublicationYear = 1997,
                    Quantity = 10,
                    CategoryId = 3,
                    Description = "Examines geographic and environmental factors that shaped the dominance of certain civilizations throughout history."
                },
                new Book
                {
                    Id = 6,
                    Title = "The Wright Brothers",
                    Author = "David McCullough",
                    ISBN = "9781476728742",
                    PublicationYear = 2015,
                    Quantity = 10,
                    CategoryId = 3,
                    Description = "A detailed biography of Orville and Wilbur Wright, chronicling their journey to inventing the first successful airplane."
                },

                // Biography books
                new Book
                {
                    Id = 7,
                    Title = "The Diary of a Young Girl",
                    Author = "Anne Frank",
                    ISBN = "9780553296983",
                    PublicationYear = 1947,
                    Quantity = 10,
                    CategoryId = 4,
                    Description = "The poignant and powerful diary of a Jewish girl hiding from the Nazis during World War II."
                },
                new Book
                {
                    Id = 8,
                    Title = "Steve Jobs",
                    Author = "Walter Isaacson",
                    ISBN = "9781451648539",
                    PublicationYear = 2011,
                    Quantity = 10,
                    CategoryId = 4,
                    Description = "The authorized biography of Apple's visionary co-founder, revealing his genius, creativity, and complex personality."
                },

                // Children books
                new Book
                {
                    Id = 9,
                    Title = "Charlotte's Web",
                    Author = "E.B. White",
                    ISBN = "9780061124952",
                    PublicationYear = 1952,
                    Quantity = 10,
                    CategoryId = 5,
                    Description = "A heartwarming tale of friendship between a pig named Wilbur and a clever spider named Charlotte."
                },
                new Book
                {
                    Id = 10,
                    Title = "The Cat in the Hat",
                    Author = "Dr. Seuss",
                    ISBN = "9780394800011",
                    PublicationYear = 1957,
                    Quantity = 10,
                    CategoryId = 5,
                    Description = "A whimsical and rhyming story about a mischievous cat who turns a rainy day into an unforgettable adventure."
                },

                // Additional Fiction books
                new Book
                {
                    Id = 11,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "9780743273565",
                    PublicationYear = 1925,
                    Quantity = 8,
                    CategoryId = 1,
                    Description = "A portrait of the Jazz Age through the tragic story of the mysterious millionaire Jay Gatsby and his love for Daisy Buchanan."
                },
                new Book
                {
                    Id = 12,
                    Title = "Dune",
                    Author = "Frank Herbert",
                    ISBN = "9780441172719",
                    PublicationYear = 1965,
                    Quantity = 7,
                    CategoryId = 1,
                    Description = "A science fiction epic about politics, religion, and power on the desert planet Arrakis, home to the valuable spice melange."
                },

                // Additional Science books
                new Book
                {
                    Id = 13,
                    Title = "Cosmos",
                    Author = "Carl Sagan",
                    ISBN = "9780345539434",
                    PublicationYear = 1980,
                    Quantity = 6,
                    CategoryId = 2,
                    Description = "A journey through the universe exploring astronomy, biology, and the history of science, connecting humanity to the cosmos."
                },
                new Book
                {
                    Id = 14,
                    Title = "The Gene: An Intimate History",
                    Author = "Siddhartha Mukherjee",
                    ISBN = "9781476733500",
                    PublicationYear = 2016,
                    Quantity = 5,
                    CategoryId = 2,
                    Description = "A comprehensive history of genetics, from its discovery to modern breakthroughs and ethical dilemmas."
                },

                // Additional History books
                new Book
                {
                    Id = 15,
                    Title = "Sapiens: A Brief History of Humankind",
                    Author = "Yuval Noah Harari",
                    ISBN = "9780062316097",
                    PublicationYear = 2011,
                    Quantity = 9,
                    CategoryId = 3,
                    Description = "Explores the history of Homo sapiens from the Stone Age to the 21st century, examining key revolutions in human development."
                },
                new Book
                {
                    Id = 16,
                    Title = "The Silk Roads: A New History of the World",
                    Author = "Peter Frankopan",
                    ISBN = "9781408839997",
                    PublicationYear = 2015,
                    Quantity = 4,
                    CategoryId = 3,
                    Description = "Reinterprets world history with Asia and the Silk Road trade routes at the center of global exchange and power."
                },

                // Additional Biography books
                new Book
                {
                    Id = 17,
                    Title = "Becoming",
                    Author = "Michelle Obama",
                    ISBN = "9781524763138",
                    PublicationYear = 2018,
                    Quantity = 7,
                    CategoryId = 4,
                    Description = "An intimate memoir by the former First Lady, chronicering her journey from Chicago's South Side to the White House."
                },
                new Book
                {
                    Id = 18,
                    Title = "Educated",
                    Author = "Tara Westover",
                    ISBN = "9780399590504",
                    PublicationYear = 2018,
                    Quantity = 6,
                    CategoryId = 4,
                    Description = "A memoir about a woman who leaves her survivalist family and goes on to earn a PhD from Cambridge University."
                },

                // Additional Children books
                new Book
                {
                    Id = 19,
                    Title = "Harry Potter and the Sorcerer's Stone",
                    Author = "J.K. Rowling",
                    ISBN = "9780590353427",
                    PublicationYear = 1997,
                    Quantity = 10,
                    CategoryId = 5,
                    Description = "The first book in the Harry Potter series, following a young boy's discovery of his magical heritage at Hogwarts School."
                },
                new Book
                {
                    Id = 20,
                    Title = "Where the Wild Things Are",
                    Author = "Maurice Sendak",
                    ISBN = "9780060254926",
                    PublicationYear = 1963,
                    Quantity = 8,
                    CategoryId = 5,
                    Description = "A classic picture book about Max's imaginative journey to the land of the Wild Things, where he becomes king."
                }
            );
        }
    }
}
