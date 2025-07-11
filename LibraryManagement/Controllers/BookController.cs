using LibraryManagement.DTOs;
using LibraryManagement.DTOs.SearchParams;
using LibraryManagement.Helpers;
using LibraryManagement.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateBookDto createBookDto)
        {
            var response = await _bookService.CreateAsync(createBookDto);
            return ControllerHelper.HandleApiResponse(response);
        }

        [HttpGet]

        public async Task<IActionResult> GetAll([FromQuery] SearchParams searchParams)
        {
            var response = await _bookService.GetAllAsync(searchParams.SearchTerm, searchParams.PageNumber, searchParams.PageSize);
            return ControllerHelper.HandleApiResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _bookService.GetByIdAsync(id);
            return ControllerHelper.HandleApiResponse(response);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBookDto updateBookDto)
        {
            var response = await _bookService.UpdateAsync(id, updateBookDto);
            return ControllerHelper.HandleApiResponse(response);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _bookService.DeleteAsync(id);
            return ControllerHelper.HandleApiResponse(response);
        }


    }
}
