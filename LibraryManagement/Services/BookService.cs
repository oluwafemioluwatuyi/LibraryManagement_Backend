using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using LibraryManagement.Constants;
using LibraryManagement.DTOs;
using LibraryManagement.DTOs.SearchParams;
using LibraryManagement.Helpers;
using LibraryManagement.Interfaces.Repositories;
using LibraryManagement.Interfaces.Services;
using LibraryManagement.Models;

namespace LibraryManagement.Services
{
    public class BookService : IBookService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IUserRepository userRepository;
        private readonly IBookRepository bookRepository;
        private readonly IMapper mapper;

        public BookService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IBookRepository bookRepository, IMapper mapper)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.userRepository = userRepository;
            this.bookRepository = bookRepository;
            this.mapper = mapper;

        }
        public async Task<ServiceResponse<object>> CreateAsync(CreateBookDto createBookDto)
        {
            // Fetch the user from the database
            var userId = httpContextAccessor.HttpContext.User.GetLoggedInUserId();

            var user = await userRepository.GetByIdAsync(userId);

            if (user is null)
            {
                return new ServiceResponse<object>(ResponseStatus.BadRequest, AppStatusCodes.ResourceNotFound, "User does not exist", null);
            }

            //  Check for duplicate book by ISBN
            var searchParams = new SearchParams
            {
                SearchTerm = createBookDto.ISBN,
                PageNumber = 1,
                PageSize = 50 // can be any safe max value
            };

            var existingBooks = await bookRepository.GetAllAsync(searchParams, userId);

            //  Check exact match (since search is fuzzy)
            if (existingBooks.Any(b => b.ISBN.Equals(createBookDto.ISBN, StringComparison.OrdinalIgnoreCase)))
            {
                return new ServiceResponse<object>(
                    ResponseStatus.BadRequest,
                    AppStatusCodes.AlreadyExists,
                    "A book with this ISBN already exists.",
                    null
                );
            }

            //  Map DTO to Book entity
            var book = mapper.Map<Book>(createBookDto);

            //  Save the book
            bookRepository.Add(book);
            var success = await bookRepository.SaveChangesAsync();

            if (!success)
            {
                return new ServiceResponse<object>(
                    ResponseStatus.Error,
                    AppStatusCodes.InternalServerError,
                    "Failed to save book.",
                    null
                );
            }

            //  Return mapped DTO
            var bookDto = mapper.Map<BookDto>(book);

            return new ServiceResponse<object>(
                ResponseStatus.Success,
                AppStatusCodes.Success,
                "Book created successfully.",
                bookDto
            );

        }
        public async Task<ServiceResponse<bool>> DeleteAsync(int id)
        {
            // Get logged-in user ID from claims
            var userId = httpContextAccessor.HttpContext.User.GetLoggedInUserId();

            //  Get the user from the database
            var user = await userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<bool>(
                    ResponseStatus.BadRequest,
                    AppStatusCodes.ResourceNotFound,
                    "User does not exist.",
                    false
                );
            }

            // Find the book to delete
            var book = await bookRepository.GetByIdAsync(id);
            if (book is null)
            {
                return new ServiceResponse<bool>(
                    ResponseStatus.NotFound,
                    AppStatusCodes.ResourceNotFound,
                    "Book not found.",
                    false
                );
            }

            // Check if the logged-in user is the creator of the book
            // if (book.UserId != user.Id)
            // {
            //     return new ServiceResponse<bool>(
            //         ResponseStatus.Unauthorized,
            //         AppStatusCodes.Unauthorized,
            //         "You are not authorized to delete this book.",
            //         false
            //     );
            // }

            //  Delete the book
            bookRepository.Remove(book);
            var success = await bookRepository.SaveChangesAsync();

            if (!success)
            {
                return new ServiceResponse<bool>(
                    ResponseStatus.Error,
                    AppStatusCodes.InternalServerError,
                    "Failed to delete the book.",
                    false
                );
            }

            return new ServiceResponse<bool>(
                ResponseStatus.Success,
                AppStatusCodes.Success,
                "Book deleted successfully.",
                true
            );
        }

        public async Task<ServiceResponse<IEnumerable<object>>> GetAllAsync(string? search = null, int pageNumber = 1, int pageSize = 10)
        {
            //  Get logged-in user ID
            var userId = httpContextAccessor.HttpContext.User.GetLoggedInUserId();

            //  Get the user from the database
            var user = await userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<IEnumerable<object>>(
                    ResponseStatus.BadRequest,
                    AppStatusCodes.ResourceNotFound,
                    "User does not exist.",
                    null
                );
            }

            //  Construct SearchParams manually
            var searchParams = new SearchParams
            {
                SearchTerm = search?.Trim() ?? string.Empty,
                PageNumber = pageNumber > 0 ? pageNumber : 1,
                PageSize = pageSize > 0 ? Math.Min(pageSize, 50) : 10
            };

            //  Query the repository
            var books = await bookRepository.GetAllAsync(searchParams, userId);

            //  Map to DTOs
            var bookDtos = mapper.Map<IEnumerable<BookDto>>(books);

            // 6. Wrap and return response
            return new ServiceResponse<IEnumerable<object>>(
                ResponseStatus.Success,
                AppStatusCodes.Success,
                "Books retrieved successfully.",
                bookDtos
            );
        }

        public async Task<ServiceResponse<object>> GetByIdAsync(int id)
        {
            //  Get logged-in user ID from claims
            var userId = httpContextAccessor.HttpContext.User.GetLoggedInUserId();

            // Get the user from the database
            var user = await userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<object>(
                    ResponseStatus.BadRequest,
                    AppStatusCodes.ResourceNotFound,
                    "User does not exist.",
                    null
                );
            }

            //  Retrieve the book
            var book = await bookRepository.GetByIdAsync(id);
            if (book is null)
            {
                return new ServiceResponse<object>(
                    ResponseStatus.NotFound,
                    AppStatusCodes.ResourceNotFound,
                    "Book not found.",
                    null
                );
            }

            // 4. Ensure the user owns the book
            // if (book.UserId != user.Id)
            // {
            //     return new ServiceResponse<object>(
            //         ResponseStatus.Unauthorized,
            //         AppStatusCodes.Unauthorized,
            //         "You are not authorized to view this book.",
            //         null
            //     );
            // }

            //  Map to DTO
            var bookDto = mapper.Map<BookDto>(book);

            return new ServiceResponse<object>(
                ResponseStatus.Success,
                AppStatusCodes.Success,
                "Book retrieved successfully.",
                bookDto
            );
        }

        public async Task<ServiceResponse<object>> UpdateAsync(int id, UpdateBookDto dto)
        {
            var userId = httpContextAccessor.HttpContext.User.GetLoggedInUserId();

            var user = await userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return new ServiceResponse<object>(
                    ResponseStatus.BadRequest,
                    AppStatusCodes.ResourceNotFound,
                    "User does not exist.",
                    null
                );
            }

            //  Get book to update
            var book = await bookRepository.GetByIdAsync(id);
            if (book is null)
            {
                return new ServiceResponse<object>(
                    ResponseStatus.NotFound,
                    AppStatusCodes.ResourceNotFound,
                    "Book not found.",
                    null
                );
            }

            // check if the user is authorized to delete the resources
            // if (book.UserId != user.Id)
            // {
            //     return new ServiceResponse<object>(
            //         ResponseStatus.Unauthorized,
            //         AppStatusCodes.Unauthorized,
            //         "You are not authorized to update this book.",
            //         null
            //     );
            // }

            // ap update fields (preserve ID and user info)
            mapper.Map(dto, book);

            // mark as modified and save
            bookRepository.MarkAsModified(book);
            var success = await bookRepository.SaveChangesAsync();

            if (!success)
            {
                return new ServiceResponse<object>(
                    ResponseStatus.Error,
                    AppStatusCodes.InternalServerError,
                    "Failed to update the book.",
                    null
                );
            }

            //return updated book DTO
            var bookDto = mapper.Map<BookDto>(book);

            return new ServiceResponse<object>(
                ResponseStatus.Success,
                AppStatusCodes.Success,
                "Book updated successfully.",
                bookDto
            );
        }
    }
}
