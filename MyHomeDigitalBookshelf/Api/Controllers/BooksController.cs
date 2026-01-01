using Microsoft.AspNetCore.Mvc;
using MyHomeDigitalBookshelf.Application.Books;
using MyHomeDigitalBookshelf.Application.Books.Commands;

namespace MyHomeDigitalBookshelf.Api.Controllers;

[ApiController]
[Route("api/v1/books")]
public class BooksController : ControllerBase
{
    private readonly BookService _bookService;

    public BooksController(BookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost("from-isbn")]
    public async Task<IActionResult> AddBookFromIsbn([FromBody] AddBookFromIsbnCommand command) // TODO: Create DTOs for commands and queries
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

    // TODO: Implement this method to retrieve a book by its ID
    [HttpGet("{id}")]
    public IActionResult GetBookById(Guid id)
    {
        return Ok($"Book with ID {id} not yet implemented.");
    }
}
