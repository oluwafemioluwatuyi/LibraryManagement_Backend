using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using LibraryManagement.Interfaces;
using LibraryManagement.Interfaces.Other;

namespace LibraryManagement.Data;

public class DbInitializer
{
    public async static Task InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<LibraryManagementDbContext>();
            await context.Database.MigrateAsync();

            var seederHandler = services.GetRequiredService<ISeederHandler>();
            await SeedData(seederHandler);
        }
        catch (Exception ex)
        {
            // Log the error (use a logging framework like Serilog or NLog)
            Console.WriteLine($"An error occurred while migrating or seeding the database: {ex.Message}");
        }
    }

    private async static Task SeedData(ISeederHandler seederHandler)
    {
        await seederHandler.seed();
    }
}
