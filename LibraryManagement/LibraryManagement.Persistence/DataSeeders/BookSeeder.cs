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
                }
            );
        }
    }
}
