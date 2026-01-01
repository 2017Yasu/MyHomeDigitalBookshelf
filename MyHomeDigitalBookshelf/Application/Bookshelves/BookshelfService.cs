using MyHomeDigitalBookshelf.Application.Common.Interfaces; // Added
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects; // Added to handle Email type

namespace MyHomeDigitalBookshelf.Application.Bookshelves;

/// <summary>
/// Service for managing bookshelves in the system.
/// </summary>
public class BookshelfService
{
    private readonly IBookshelfRepository _bookshelfRepository;
    private readonly IBookshelfUserRepository _bookshelfUserRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService; // Added

    public BookshelfService(
        IBookshelfRepository bookshelfRepository,
        IBookshelfUserRepository bookshelfUserRepository,
        IUserRepository userRepository,
        IEmailService emailService) // Added
    {
        _bookshelfRepository = bookshelfRepository ?? throw new ArgumentNullException(nameof(bookshelfRepository));
        _bookshelfUserRepository = bookshelfUserRepository ?? throw new ArgumentNullException(nameof(bookshelfUserRepository));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService)); // Added
    }

    /// <summary>
    /// Invites a user to a bookshelf.
    /// </summary>
    /// <param name="command">The command containing bookshelf ID, invited user's email, and inviting user's ID.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task InviteUserToBookshelfAsync(Commands.InviteUserToBookshelfCommand command)
    {
        command.Validate();

        var bookshelf = await _bookshelfRepository.GetByIdAsync(command.BookshelfId);
        if (bookshelf == null)
        {
            throw new ArgumentException($"Bookshelf with ID {command.BookshelfId} not found.", nameof(command.BookshelfId));
        }

        var invitingUser = await _userRepository.GetByIdAsync(command.InvitingUserId);
        if (invitingUser == null)
        {
            throw new ArgumentException($"Inviting user with ID {command.InvitingUserId} not found.", nameof(command.InvitingUserId));
        }

        var invitedUser = await _userRepository.GetByEmailAsync(new Email(command.InvitedUserEmail));
        if (invitedUser == null)
        {
            // User not yet registered, send invite to register
            await _emailService.SendEmailAsync(
                command.InvitedUserEmail,
                "Invitation to MyHomeDigitalBookshelf",
                $"Hello,\n\nYou have been invited by {invitingUser.Username} to join the bookshelf '{bookshelf.Name}'. Please register at [App Registration Link] to accept the invitation."
            );
            return;
        }

        // User is already registered
        var existingRelationship = await _bookshelfUserRepository.GetAsync(invitedUser.Id, command.BookshelfId);
        if (existingRelationship != null)
        {
            throw new InvalidOperationException($"User {invitedUser.Email} is already a member of bookshelf {bookshelf.Name}");
        }

        var bookshelfUser = BookshelfUser.CreateNew(
            invitedUser.Id,
            bookshelf.Id,
            BookshelfUserRole.Member); // Invited users join as members initially

        await _bookshelfUserRepository.AddAsync(bookshelfUser);

        await _emailService.SendEmailAsync(
            command.InvitedUserEmail,
            "Invitation to MyHomeDigitalBookshelf",
            $"Hello {invitedUser.Username},\n\nYou have been invited by {invitingUser.Username} to join the bookshelf '{bookshelf.Name}'. You are now a member!"
        );
    }

    /// <summary>
    /// Creates a new bookshelf and assigns the owner as an administrator.
    /// </summary>
    /// <param name="command">The command containing the new bookshelf's details and owner.</param>
    /// <returns>The created bookshelf.</returns>
    public async Task<Bookshelf> CreateBookshelfForUserAsync(Commands.CreateBookshelfCommand command)
    {
        command.Validate();

        var owner = await _userRepository.GetByIdAsync(command.OwnerId);
        if (owner == null)
        {
            throw new ArgumentException($"User with ID {command.OwnerId} not found.", nameof(command.OwnerId));
        }

        // Note: In a real application, this should be a single database transaction.
        // Assuming the repository implementation handles transactions or this is handled at a higher level (e.g., Unit of Work).

        var bookshelf = Bookshelf.CreateNew(
            command.Name,
            command.Description);

        await _bookshelfRepository.AddAsync(bookshelf);

        var bookshelfUser = BookshelfUser.CreateNew(
            command.OwnerId,
            bookshelf.Id,
            BookshelfUserRole.Admin);

        await _bookshelfUserRepository.AddAsync(bookshelfUser);

        return bookshelf;
    }

    /// <summary>
    /// Adds a new bookshelf to the system.
    /// </summary>
    /// <param name="command">The command containing bookshelf details.</param>
    /// <returns>The created bookshelf.</returns>
    public async Task<Bookshelf> AddBookshelfAsync(Commands.AddBookshelfCommand command)
    {
        command.Validate();

        var bookshelf = Bookshelf.CreateNew(
            command.Name,
            command.Description);

        return await _bookshelfRepository.AddAsync(bookshelf);
    }

    /// <summary>
    /// Updates an existing bookshelf in the system.
    /// </summary>
    /// <param name="command">The command containing updated bookshelf details.</param>
    /// <returns>The updated bookshelf, or null if the bookshelf was not found.</returns>
    public async Task<Bookshelf?> UpdateBookshelfAsync(Commands.UpdateBookshelfCommand command)
    {
        command.Validate();

        var existingBookshelf = await _bookshelfRepository.GetByIdAsync(command.Id);
        if (existingBookshelf == null)
        {
            return null;
        }

        var updatedBookshelf = new Bookshelf(
            command.Id,
            command.Name,
            command.Description,
            existingBookshelf.CreatedAt,
            DateTime.UtcNow);

        return await _bookshelfRepository.UpdateAsync(updatedBookshelf);
    }

    /// <summary>
    /// Deletes a bookshelf from the system.
    /// </summary>
    /// <param name="command">The command containing the bookshelf ID to delete.</param>
    public async Task DeleteBookshelfAsync(Commands.DeleteBookshelfCommand command)
    {
        command.Validate();
        await _bookshelfRepository.DeleteAsync(command.Id);
    }

    /// <summary>
    /// Gets a bookshelf by its ID.
    /// </summary>
    /// <param name="query">The query containing the bookshelf ID.</param>
    /// <returns>The bookshelf if found; otherwise, null.</returns>
    public async Task<Bookshelf?> GetBookshelfByIdAsync(Queries.GetBookshelfByIdQuery query)
    {
        query.Validate();
        return await _bookshelfRepository.GetByIdAsync(query.Id);
    }

    /// <summary>
    /// Gets all bookshelves in the system.
    /// </summary>
    /// <param name="query">The query with no parameters.</param>
    /// <returns>An array of all bookshelves.</returns>
    public async Task<Bookshelf[]> GetAllBookshelvesAsync(Queries.GetAllBookshelvesQuery query)
    {
        query.Validate();
        return await _bookshelfRepository.GetAllAsync();
    }

    /// <summary>
    /// Gets all bookshelves that a user belongs to.
    /// </summary>
    /// <param name="query">The query containing the user ID.</param>
    /// <returns>An array of BookshelfUser relationships for the user.</returns>
    public async Task<BookshelfUser[]> GetUserBookshelvesAsync(Queries.GetUserBookshelvesQuery query)
    {
        query.Validate();

        // Validate that the user exists
        var user = await _userRepository.GetByIdAsync(query.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {query.UserId} not found.", nameof(query.UserId));
        }

        return await _bookshelfUserRepository.GetByUserAsync(query.UserId);
    }

    /// <summary>
    /// Gets all members (users) of a bookshelf.
    /// </summary>
    /// <param name="query">The query containing the bookshelf ID.</param>
    /// <returns>An array of BookshelfUser relationships for the bookshelf.</returns>
    public async Task<BookshelfUser[]> GetBookshelfUsersAsync(Queries.GetBookshelfUsersQuery query)
    {
        query.Validate();

        // Validate that the bookshelf exists
        var bookshelf = await _bookshelfRepository.GetByIdAsync(query.BookshelfId);
        if (bookshelf == null)
        {
            throw new ArgumentException($"Bookshelf with ID {query.BookshelfId} not found.", nameof(query.BookshelfId));
        }

        return await _bookshelfUserRepository.GetByBookshelfAsync(query.BookshelfId);
    }

    /// <summary>
    /// Adds a user to a bookshelf with the specified role.
    /// </summary>
    /// <param name="command">The command containing user-bookshelf relationship details.</param>
    /// <returns>The created BookshelfUser relationship.</returns>
    public async Task<BookshelfUser> AddBookshelfUserAsync(Commands.AddBookshelfUserCommand command)
    {
        command.Validate();

        // Check if the relationship already exists
        var existingRelationship = await _bookshelfUserRepository.GetAsync(command.UserId, command.BookshelfId);
        if (existingRelationship != null)
        {
            throw new InvalidOperationException($"User {command.UserId} is already a member of bookshelf {command.BookshelfId}");
        }

        // Validate that the user exists
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {command.UserId} not found.", nameof(command.UserId));
        }

        // Validate that the bookshelf exists
        var bookshelf = await _bookshelfRepository.GetByIdAsync(command.BookshelfId);
        if (bookshelf == null)
        {
            throw new ArgumentException($"Bookshelf with ID {command.BookshelfId} not found.", nameof(command.BookshelfId));
        }

        var bookshelfUser = BookshelfUser.CreateNew(
            command.UserId,
            command.BookshelfId,
            command.Role);

        return await _bookshelfUserRepository.AddAsync(bookshelfUser);
    }

    /// <summary>
    /// Updates a user's role in a bookshelf.
    /// </summary>
    /// <param name="command">The command containing the updated role.</param>
    /// <returns>The updated BookshelfUser relationship, or null if the relationship was not found.</returns>
    public async Task<BookshelfUser?> UpdateBookshelfUserAsync(Commands.UpdateBookshelfUserCommand command)
    {
        command.Validate();

        var existingRelationship = await _bookshelfUserRepository.GetAsync(command.UserId, command.BookshelfId);
        if (existingRelationship == null)
        {
            return null;
        }

        var updatedBookshelfUser = new BookshelfUser(
            command.UserId,
            command.BookshelfId,
            command.NewRole,
            existingRelationship.CreatedAt,
            DateTime.UtcNow);

        return await _bookshelfUserRepository.UpdateAsync(updatedBookshelfUser);
    }

    /// <summary>
    /// Removes a user from a bookshelf.
    /// </summary>
    /// <param name="command">The command containing the user and bookshelf IDs.</param>
    public async Task RemoveBookshelfUserAsync(Commands.RemoveBookshelfUserCommand command)
    {
        command.Validate();

        // Verify the relationship exists before trying to delete it
        var existingRelationship = await _bookshelfUserRepository.GetAsync(command.UserId, command.BookshelfId);
        if (existingRelationship == null)
        {
            throw new ArgumentException($"User {command.UserId} is not a member of bookshelf {command.BookshelfId}");
        }

        await _bookshelfUserRepository.DeleteAsync(command.UserId, command.BookshelfId);
    }
}
