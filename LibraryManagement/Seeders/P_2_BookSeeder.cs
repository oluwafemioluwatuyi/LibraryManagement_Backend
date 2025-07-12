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
