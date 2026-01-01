using Microsoft.AspNetCore.Mvc;
using MyHomeDigitalBookshelf.Application.Books;
using MyHomeDigitalBookshelf.Application.Books.Commands;

namespace MyHomeDigitalBookshelf.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost("from-isbn")]
    public async Task<IActionResult> AddBookFromIsbn([FromBody] AddBookFromIsbnCommand command)
    {
        try
        {
            var book = await _bookService.AddBookFromIsbnAsync(command);
            return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // Placeholder for GetBookById - will be implemented later
    [HttpGet("{id}")]
    public IActionResult GetBookById(Guid id)
    {
        return Ok($"Book with ID {id} not yet implemented.");
    }
}
