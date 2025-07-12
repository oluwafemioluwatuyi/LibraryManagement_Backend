using LibraryManagement.Data;
using LibraryManagement.DTOs.SearchParams;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryManagementDbContext _dbContext;
        public BookRepository(LibraryManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Add(Book book)
        {
            _dbContext.Books.Add(book);
        }

        public async Task<IEnumerable<Book>> GetAllAsync(SearchParams searchParams, int? userId)
        {
            var query = _dbContext.Books.AsQueryable();

            // 1. Filter by the logged-in user
            // query = query.Where(b => b.UserId == userId);

            // Search by title or author
            if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
            {
                var keyword = searchParams.SearchTerm.ToLower();
                query = query.Where(b =>
                    b.Title.ToLower().Contains(keyword) ||
                    b.Author.ToLower().Contains(keyword));
            }

            // 3. Apply pagination (already validated by model binder)
            query = query
                .Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                .Take(searchParams.PageSize);

            return await query.ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(u => u.Id == id);
        }

        public void MarkAsModified(Book book)
        {
            _dbContext.Entry(book).State = EntityState.Modified;

        }

        public void Remove(Book book)
        {
            _dbContext.Books.Remove(book);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;

        }
    }
}
