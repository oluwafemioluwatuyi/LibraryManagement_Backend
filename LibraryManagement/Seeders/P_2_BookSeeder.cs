using LibraryManagement.Interfaces.Other;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Models;

namespace LibraryManagement.Seeders
{
    public class P_2_BookSeeder : ISeeder
    {
        private readonly IBookRepository _bookRepository;
        private readonly IConstants _constants;
        public P_2_BookSeeder(IBookRepository bookRepository, IConstants constants)
        {
            _bookRepository = bookRepository;
            _constants = constants;
        }
        public Task up()
        {
            Console.WriteLine("Seeding books...");

            var books = new List<Book>
            {
                new Book
                {
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    ISBN = "9780743273565",
                    PublishedDate = new DateTime(1925, 4, 10),

                },
                 new Book
                {
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    ISBN = "9780061120084",
                    PublishedDate = new DateTime(1960, 7, 11)
                },
                new Book
                {
                    Title = "1984",
                    Author = "George Orwell",
                    ISBN = "9780451524935",
                    PublishedDate = new DateTime(1949, 6, 8)
                },
                new Book
                {
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    ISBN = "9780141439518",
                    PublishedDate = new DateTime(1813, 1, 28)
                }
            };

            foreach (var book in books)
            {
                _bookRepository.Add(book);
            }
            return _bookRepository.SaveChangesAsync();
        }
        public Task down()
        {
            throw new NotImplementedException();
        }


        public string Description()
        {
            throw new NotImplementedException();
        }


    }
}
