using System.ComponentModel;
using System.Text.Json;
using AutoMapper;
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


builder.Services.AddControllers(options =>
{
    // TODO: Find out why this is not working and causes an error to be returned to the client
    // options.Filters.Add<ValidateMonnifySignatureAttribute>();

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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                // Check if the endpoint requires authentication
                var endpoint = context.HttpContext.GetEndpoint();
                var requiresAuth = endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null;

                if (!requiresAuth)
                {
                    // Skip authentication if the endpoint does not require it
                    context.NoResult();
                }

                return System.Threading.Tasks.Task.CompletedTask;
            },
            OnAuthenticationFailed = async context =>
            {
                // This should handle cases like invalid tokens or malformed tokens.
                if (context.Exception is SecurityTokenExpiredException)
                {
                    context.Response.Headers.Append("Token-Expired", "true");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var responseBody = JsonConvert.SerializeObject(new ServiceResponse<object>(ResponseStatus.Unauthorized, AppStatusCodes.Unauthorized, "Token expired", null));
                    await context.Response.WriteAsync(responseBody);
                }
                else
                {
                    context.Response.Headers.Append("Authentication-Failed", "true");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";
                    var responseBody = JsonConvert.SerializeObject(new ServiceResponse<object>(ResponseStatus.Unauthorized, AppStatusCodes.Unauthorized, "Authentication failed", null));
                    await context.Response.WriteAsync(responseBody);
                }
                return;
            },
            OnChallenge = async context =>
            {
                // This should handle cases where no authentication was provided.
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var responseBody = JsonConvert.SerializeObject(new ServiceResponse<object>(ResponseStatus.Unauthorized, AppStatusCodes.Unauthorized, "Unauthorized", null));
                await context.Response.WriteAsync(responseBody);
                return;
            },
            OnForbidden = async context =>
            {
                // This handles cases where authentication succeeded but the user lacks permissions.
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                var responseBody = JsonConvert.SerializeObject(new ServiceResponse<object>(ResponseStatus.Forbidden, AppStatusCodes.Unauthorized, "You are not allowed to use this feature", null));
                await context.Response.WriteAsync(responseBody);
                return;
            }
        };

    });




builder.Services.AddHttpContextAccessor();



builder.Services.AddDbContext<LibraryManagementDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerDatabase"));
});

// Ensure you have a MappingProfile class defined in your project, for example in LibraryManagement.Helpers or a similar namespace.
// If you don't have one, create it as shown below, or replace 'MappingProfile' with the correct profile class name.
//builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);






// builder.Services
// .AddFluentValidationAutoValidation();

//Constants
builder.Services.AddScoped<IConstants, Constants>();

// Seeders
builder.Services.AddScoped<P_1_UserSeeder>();
builder.Services.AddScoped<P_2_BookSeeder>();

//Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBookService, BookService>();


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();


// try
// {
//     await DbInitializer.InitDb(app);
// }
// catch (Exception e)
// {
//     Console.WriteLine(e);
// }



app.Run();
