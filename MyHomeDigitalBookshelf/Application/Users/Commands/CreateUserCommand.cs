using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Users.Commands;

public class CreateUserCommand
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Username))
        {
            throw new ArgumentException("Username must not be null or whitespace.", nameof(Username));
        }
        if (string.IsNullOrWhiteSpace(Password))
        {
            throw new ArgumentException("Password must not be null or whitespace.", nameof(Password));
        }
        // Email validation is handled by the Email value object constructor
        _ = new Email(Email);
    }
}
