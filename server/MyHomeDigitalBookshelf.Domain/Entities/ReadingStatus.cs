namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents the reading status of a book for a user.
/// </summary>
public enum ReadingStatus
{
    /// <summary>
    /// The book has not been read yet.
    /// </summary>
    Unread,

    /// <summary>
    /// The user wants to read the book.
    /// </summary>
    WantToRead,

    /// <summary>
    /// The book is currently being read.
    /// </summary>
    Reading,

    /// <summary>
    /// The book has been completed.
    /// </summary>
    Completed,
}
