namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Defines the possible roles a user can have within a bookshelf.
/// </summary>
public enum BookshelfUserRole
{
    /// <summary>
    /// Member: Can add/edit owned books in the bookshelf, update statuses, search entire library.
    /// </summary>
    Member,

    /// <summary>
    /// Administrator: Can manage members to access the bookshelf, full access to books and categories in the bookshelf
    /// </summary>
    Administrator,
}
