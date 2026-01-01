using Microsoft.AspNetCore.Mvc;
using MyHomeDigitalBookshelf.Application.UserBooks;
using MyHomeDigitalBookshelf.Application.UserBooks.Commands;
using System.Security.Claims; // For HttpContext.User

namespace MyHomeDigitalBookshelf.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserBooksController : ControllerBase
{
    private readonly UserBookService _userBookService;

    public UserBooksController(UserBookService userBookService)
    {
        _userBookService = userBookService;
    }

    [HttpPut("{bookId}/status")]
    public async Task<IActionResult> UpdateBookStatus(Guid bookId, [FromBody] UpdateUserBookStatusCommand command)
    {
        // For now, assume UserId is passed directly or can be retrieved from an authenticated context.
        // In a real application, this would come from HttpContext.User.Claims
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
