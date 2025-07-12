using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryManagement.Interfaces;
using LibraryManagement.Interfaces.Other;
using LibraryManagement.Seeders;
using LibraryManagement.Constants;

namespace LibraryManagement.Data;

public class DbInitializer
{
    private readonly IEnumerable<ISeeder> _seeders;

    public DbInitializer(IEnumerable<ISeeder> seeders)
    {
        _seeders = seeders.OrderBy(s => s switch
        {
            P_1_UserSeeder => 1,
            P_2_BookSeeder => 2,
            _ => 10
        });
    }
    public static void InitializeDatabase(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<LibraryManagementDbContext>();
        context.Database.Migrate();
        Console.WriteLine("Database migration completed.");
        // Check if the database is already seeded
        if (context.Users.AnyAsync(u => u.Email == services.GetRequiredService<IConstants>().SYSTEM_USER_EMAIL).Result)
        {
            Console.WriteLine($"Database already seeded (system user found). Skipping seeding.");
            return;
        }

        var initializer = services.GetRequiredService<DbInitializer>();
        foreach (var seeder in initializer._seeders)
        {
            seeder.up().Wait();
        }
    }
}
