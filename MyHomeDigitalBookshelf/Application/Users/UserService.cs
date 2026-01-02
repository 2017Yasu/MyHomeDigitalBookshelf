using MyHomeDigitalBookshelf.Application.Common.Interfaces;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Application.Users;

/// <summary>
/// Service for managing users in the system.
/// </summary>
[SingletonService]
public class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserIdentityRepository _userIdentityRepository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IUserIdentityRepository userIdentityRepository,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _userIdentityRepository = userIdentityRepository ?? throw new ArgumentNullException(nameof(userIdentityRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }

    /// <summary>
    /// Creates a new user with a password.
    /// </summary>
    /// <param name="command">The command containing the new user's details.</param>
    /// <returns>The created user.</returns>
    public async Task<User> CreateUserAsync(Commands.CreateUserCommand command)
    {
        command.Validate();

        var email = new Email(command.Email);
        var existingUserByEmail = await _userRepository.GetByEmailAsync(email.Value);
        if (existingUserByEmail != null)
        {
            throw new InvalidOperationException($"Email {command.Email} is already registered.");
        }

        var existingUserByUsername = await _userRepository.GetByUsernameAsync(command.Username);
        if (existingUserByUsername != null)
        {
            throw new InvalidOperationException($"Username {command.Username} is already taken.");
        }

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.CreateNew(
            command.Username,
            email,
            passwordHash,
            UserRole.Member);

        return await _userRepository.AddAsync(user);
    }

    /// <summary>
    /// Authenticates a user by email and password.
    /// </summary>
    /// <param name="query">The query containing the user's credentials.</param>
    /// <returns>The authenticated user, or null if authentication fails.</returns>
    public async Task<User?> AuthenticateUserAsync(Queries.AuthenticateUserQuery query)
    {
        query.Validate();

        var email = new Email(query.Email);
        var user = await _userRepository.GetByEmailAsync(email.Value); // Corrected

        if (user == null || user.PasswordHash == null)
        {
            return null; // User not found or has no password (e.g., external login)
        }

        if (!_passwordHasher.Verify(query.Password, user.PasswordHash))
        {
            return null; // Invalid password
        }

        return user;
    }

    /// <summary>
    /// Adds a new user to the system.
    /// </summary>
    /// <param name="command">The command containing user details.</param>
    /// <returns>The created user.</returns>
    public async Task<User> AddUserAsync(Commands.AddUserCommand command)
    {
        command.Validate();

        // Check if username is already taken
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(command.Username);
        if (existingUserByUsername != null)
        {
            throw new InvalidOperationException($"Username {command.Username} is already taken.");
        }

        // Check if email is already taken (if provided)
        if (command.Email != null)
        {
            var existingUserByEmail = await _userRepository.GetByEmailAsync(command.Email.Value);
            if (existingUserByEmail != null)
            {
                throw new InvalidOperationException($"Email {command.Email} is already registered.");
            }
        }

        var user = User.CreateNew(
            command.Username,
            command.Email,
            command.PasswordHash,
            command.Role);

        return await _userRepository.AddAsync(user);
    }

    /// <summary>
    /// Updates an existing user in the system.
    /// </summary>
    /// <param name="command">The command containing updated user details.</param>
    /// <returns>The updated user, or null if the user was not found.</returns>
    public async Task<User?> UpdateUserAsync(Commands.UpdateUserCommand command)
    {
        command.Validate();

        var existingUser = await _userRepository.GetByIdAsync(command.Id);
        if (existingUser == null)
        {
            return null;
        }

        // Check if new username is already taken by another user
        var existingUserByUsername = await _userRepository.GetByUsernameAsync(command.Username);
        if (existingUserByUsername != null && existingUserByUsername.Id != command.Id)
        {
            throw new InvalidOperationException($"Username {command.Username} is already taken.");
        }

        // Check if new email is already taken by another user (if provided)
        if (command.Email != null)
        {
            var existingUserByEmail = await _userRepository.GetByEmailAsync(command.Email.Value);
            if (existingUserByEmail != null && existingUserByEmail.Id != command.Id)
            {
                throw new InvalidOperationException($"Email {command.Email} is already registered.");
            }
        }

        var updatedUser = new User(
            command.Id,
            command.Username,
            command.Email,
            command.PasswordHash,
            command.Role,
            existingUser.CreatedAt);

        return await _userRepository.UpdateAsync(updatedUser);
    }

    /// <summary>
    /// Deletes a user from the system.
    /// </summary>
    /// <param name="command">The command containing the user ID to delete.</param>
    public async Task DeleteUserAsync(Commands.DeleteUserCommand command)
    {
        command.Validate();
        await _userRepository.DeleteAsync(command.Id);
    }

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="query">The query containing the user ID.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetUserByIdAsync(Queries.GetUserByIdQuery query)
    {
        query.Validate();
        return await _userRepository.GetByIdAsync(query.Id);
    }

    /// <summary>
    /// Gets a user by their username.
    /// </summary>
    /// <param name="query">The query containing the username.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetUserByUsernameAsync(Queries.GetUserByUsernameQuery query)
    {
        query.Validate();
        return await _userRepository.GetByUsernameAsync(query.Username);
    }

    /// <summary>
    /// Gets a user by their email address.
    /// </summary>
    /// <param name="query">The query containing the email address.</param>
    /// <returns>The user if found; otherwise, null.</returns>
    public async Task<User?> GetUserByEmailAsync(Queries.GetUserByEmailQuery query)
    {
        query.Validate();
        return await _userRepository.GetByEmailAsync(query.Email);
    }

    /// <summary>
    /// Gets all users in the system.
    /// </summary>
    /// <param name="query">The query with no parameters.</param>
    /// <returns>An array of all users.</returns>
    public async Task<User[]> GetAllUsersAsync(Queries.GetAllUsersQuery query)
    {
        query.Validate();
        return await _userRepository.GetAllAsync();
    }

    /// <summary>
    /// Gets a user identity by provider and subject.
    /// </summary>
    /// <param name="query">The query containing provider and subject.</param>
    /// <returns>The user identity if found; otherwise, null.</returns>
    public async Task<UserIdentity?> GetUserIdentityByProviderAsync(Queries.GetUserIdentityByProviderQuery query)
    {
        query.Validate();
        return await _userIdentityRepository.GetByProviderAndSubjectAsync(query.Provider, query.Subject);
    }

    /// <summary>
    /// Adds a new user identity to the system.
    /// </summary>
    /// <param name="command">The command containing user identity details.</param>
    /// <returns>The created user identity.</returns>
    public async Task<UserIdentity> AddUserIdentityAsync(Commands.AddUserIdentityCommand command)
    {
        command.Validate();

        // Check if user exists
        var user = await _userRepository.GetByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {command.UserId} not found.", nameof(command.UserId));
        }

        // Check if provider/subject combination already exists
        var existingIdentity = await _userIdentityRepository.GetByProviderAndSubjectAsync(command.Provider, command.Subject);
        if (existingIdentity != null)
        {
            throw new InvalidOperationException($"Identity with provider {command.Provider} and subject {command.Subject} already exists.");
        }

        var email = !string.IsNullOrWhiteSpace(command.Email) ? new Email(command.Email) : null;

        var identity = UserIdentity.CreateNew(
            command.UserId,
            command.Provider,
            command.Subject,
            email);

        return await _userIdentityRepository.AddAsync(identity);
    }
}
