﻿using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data
{
    public class LibraryManagementDbContext : DbContext
    {
        public LibraryManagementDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

    }
}
