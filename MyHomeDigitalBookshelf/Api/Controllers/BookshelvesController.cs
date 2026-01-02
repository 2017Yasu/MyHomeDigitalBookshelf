using Microsoft.AspNetCore.Mvc;
using MyHomeDigitalBookshelf.Application.Books;
using MyHomeDigitalBookshelf.Application.Books.Queries;
using MyHomeDigitalBookshelf.Application.Bookshelves;
using MyHomeDigitalBookshelf.Application.Bookshelves.Commands;
using MyHomeDigitalBookshelf.Application.Bookshelves.Queries;

namespace MyHomeDigitalBookshelf.Api.Controllers;

[ApiController]
[Route("api/v1/bookshelves")]
public class BookshelvesController : ControllerBase
{
    private readonly BookshelfService _bookshelfService;
    private readonly BookService _bookService;

    public BookshelvesController(BookshelfService bookshelfService, BookService bookService)
    {
        _bookshelfService = bookshelfService;
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBookshelf([FromBody] CreateBookshelfCommand command) // TODO: Create DTOs for commands and queries
    {
        try
        {
            // TODO: Retrieve OwnerId from authenticated user context
            // For now, command.OwnerId needs to be set by the client. This should be adjusted for real auth.
            var bookshelf = await _bookshelfService.CreateBookshelfForUserAsync(command);
            return CreatedAtAction(nameof(GetBookshelfById), new { id = bookshelf.Id }, bookshelf);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookshelfById(Guid id)
    {
        var query = new GetBookshelfByIdQuery { Id = id };
        try
        {
            var bookshelf = await _bookshelfService.GetBookshelfByIdAsync(query);
            if (bookshelf == null)
            {
                return NotFound();
            }
            return Ok(bookshelf);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{bookshelfId}/books")]
    public async Task<IActionResult> GetBooksForBookshelf(Guid bookshelfId, [FromQuery] GetBooksForBookshelfQuery query) // TODO: Create DTOs for commands and queries
    {
        query.BookshelfId = bookshelfId; // Ensure the bookshelfId from the route is used
        try
        {
            var books = await _bookService.GetBooksForBookshelfAsync(query);
            return Ok(books);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/members")]
    public async Task<IActionResult> InviteUserToBookshelf(Guid id, [FromBody] InviteUserToBookshelfCommand command) // TODO: Create DTOs for commands and queries
    {
        command.BookshelfId = id; // Ensure the bookshelfId from the route is used
        // TODO: Retrieve InvitingUserId from authenticated user context
        // For now, command.InvitingUserId needs to be set by the client. This should be adjusted for real auth.
        command.InvitingUserId = Guid.NewGuid(); // Placeholder

        try
        {
            await _bookshelfService.InviteUserToBookshelfAsync(command);
            return Ok(new { message = "Invitation sent successfully." });
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
}
