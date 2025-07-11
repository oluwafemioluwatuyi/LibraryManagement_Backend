using LibraryManagement.DTOs.SearchParams;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Interfaces.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllAsync(SearchParams searchParams, Guid? userId);
        void Add(Book book);
        void Remove(Book book);
        void MarkAsModified(Book book);
        Task<bool> SaveChangesAsync();

    }
}
