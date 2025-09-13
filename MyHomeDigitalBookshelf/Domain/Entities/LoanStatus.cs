namespace MyHomeDigitalBookshelf.Domain.Entities;

/// <summary>
/// Represents the loan status of a book for a user.
/// </summary>
public enum LoanStatus
{
    /// <summary>
    /// The book is not currently on loan.
    /// </summary>
    None,

    /// <summary>
    /// The book is lent out to someone else.
    /// </summary>
    Lent,

    /// <summary>
    /// The book is borrowed from someone else.
    /// </summary>
    Borrowed,
}
