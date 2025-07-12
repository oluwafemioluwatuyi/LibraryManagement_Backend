using LibraryManagement.Data;
using LibraryManagement.Interfaces.Other;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LibraryManagementDbContext _dbContext;
        private readonly IConstants _constants;
        public UserRepository(LibraryManagementDbContext dbContext, IConstants constants)
        {
            _dbContext = dbContext;
            _constants = constants;

        }
        public void Add(User user)
        {
            _dbContext.Users.Add(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetSystemUser()
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == _constants.SYSTEM_USER_EMAIL);
        }

        public void MarkAsModified(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
        }

        public void Remove(User user)
        {
            _dbContext.Users.Remove(user);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
