using AutoMapper;
using System.ComponentModel;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using LibraryManagement.Data;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Interfaces.Services;
using LibraryManagement.Repositories;
using LibraryManagement.Seeders;
using LibraryManagement.Services;
using LibraryManagement.Interfaces.Other;
using LibraryManagement.Constants;
using System.Text;
using Newtonsoft.Json;
using LibraryManagement.Helpers;


var builder = WebApplication.CreateBuilder(args);

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


builder.Services.AddControllers(options =>
{


})
           .AddJsonOptions(options =>
    {
        // System.Text.Json configuration
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
        // options.JsonSerializerOptions.Converters.Add(new SystemTextJsonDateTimeConverter()); // Custom DateTime converter for System.Text.Json
    })
    .AddNewtonsoftJson(options =>
    {
        // Newtonsoft.Json configuration
        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter { AllowIntegerValues = true });
        // options.SerializerSettings.Converters.Add(new MultiFormatDateTimeConverter()); // Custom DateTime converter for Newtonsoft.Json
    });

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

    });


builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<LibraryManagementDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDatabase"));
});

// AutoMapper configuration
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Constants
builder.Services.AddScoped<IConstants, Constants>();


//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookService, BookService>();
// builder.Services.AddSingleton<IEmailService>(provider =>
// {
//     var logger = provider.GetRequiredService<ILogger<EmailService>>();
//     var environment = provider.GetRequiredService<IWebHostEnvironment>();

//     var templatesFolderPath = Path.Combine(environment.ContentRootPath, "Emails");

//     return new EmailService(templatesFolderPath, logger, builder.Configuration);
// });

// Register seeders
builder.Services.AddScoped<ISeeder, P_1_UserSeeder>();
builder.Services.AddScoped<ISeeder, P_2_BookSeeder>();


// Register DbInitializer
builder.Services.AddScoped<DbInitializer>();

// swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new() { Title = "LibraryManagement API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer {your token}'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryManagement API v1");
    c.RoutePrefix = "swagger";

});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Run seeders
DbInitializer.InitializeDatabase(app);



app.Run();
