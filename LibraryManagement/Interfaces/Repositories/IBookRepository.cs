using LibraryManagement.DTOs.SearchParams;
using LibraryManagement.Models;


namespace LibraryManagement.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync(SearchParams searchParams, int? userId);
        void Add(Book book);
        void Remove(Book book);
        void MarkAsModified(Book book);
        Task<bool> SaveChangesAsync();

    }
}
