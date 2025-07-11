using LibraryManagement.DTOs;
using LibraryManagement.Helpers;

namespace LibraryManagement.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ServiceResponse<object>> RegisterAsync(RegisterRequestDto registerRequestDto); // returns JWT token or success message
        Task<ServiceResponse<object>> LoginAsync(LoginDto dto);

    }
}
