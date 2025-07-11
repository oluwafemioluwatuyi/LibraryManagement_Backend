using LibraryManagement.DTOs;
using LibraryManagement.Helpers;

namespace LibraryManagement.Interfaces.Services
{
    public interface IBookService
    {
        Task<ServiceResponse<IEnumerable<object>>> GetAllAsync(string? search = null, int pageNumber = 1, int pageSize = 10);
        Task<ServiceResponse<object>> GetByIdAsync(int id);
        Task<ServiceResponse<object>> CreateAsync(CreateBookDto createBookDto);
        Task<ServiceResponse<object>> UpdateAsync(int id, UpdateBookDto updateBookDto);
        Task<ServiceResponse<bool>> DeleteAsync(int id);
    }
}
