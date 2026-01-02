using Microsoft.AspNetCore.Mvc;
using MyHomeDigitalBookshelf.Application.UserBooks;
using MyHomeDigitalBookshelf.Application.UserBooks.Commands;

namespace MyHomeDigitalBookshelf.Api.Controllers;

[ApiController]
[Route("api/v1/user-books")]
public class UserBooksController : ControllerBase
{
    private readonly UserBookService _userBookService;

    public UserBooksController(UserBookService userBookService)
    {
        _userBookService = userBookService;
    }

    [HttpPut("{bookId}/status")]
    public async Task<IActionResult> UpdateBookStatus(Guid bookId, [FromBody] UpdateUserBookStatusCommand command) // TODO: Create DTOs for commands and queries
    {
        // TODO: Retrieve UserId from authenticated user context
        // Placeholder for authenticated UserId
        var userId = Guid.NewGuid(); // Replace with actual authenticated user ID

        command.UserId = userId;
        command.BookId = bookId; // Ensure bookId from route matches command

        try
        {
            var userBook = await _userBookService.UpdateUserBookStatusAsync(command);
            if (userBook == null)
            {
                return NotFound(new { message = "User book entry not found." });
            }
            return Ok(userBook);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
