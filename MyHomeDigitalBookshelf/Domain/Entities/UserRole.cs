namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Defines the possible roles a user can have in the system.
/// </summary>
public enum UserRole
{
    /// <summary>
    /// Guest: Can search and view public book lists only.
    /// </summary>
    Guest,

    /// <summary>
    /// Member: Can add/edit owned books, update statuses, search entire library.
    /// </summary>
    Member,

    /// <summary>
    /// Administrator: Has full access to all bookshelves, members, categories, and system settings.
    /// </summary>
    Administrator,
}
