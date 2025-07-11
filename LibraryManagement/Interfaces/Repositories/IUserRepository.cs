using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagement.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        void MarkAsModified(User user);
        void Remove(User user);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetSystemUser();
        void Add(User user);
        Task<bool> SaveChangesAsync();


    }
}
