using Moq;
using MyHomeDigitalBookshelf.Application.UserBooks.Commands;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using Xunit;
using System;
using System.Threading.Tasks;
using MyHomeDigitalBookshelf.Application.UserBooks;
using MyHomeDigitalBookshelf.Domain.ValueObjects; // Added for Price

namespace MyHomeDigitalBookshelf.Application.Tests.UserBooks;

public class UserBookServiceTests
{
    private readonly Mock<IUserBookRepository> _mockUserBookRepository;
    private readonly UserBookService _userBookService;

    public UserBookServiceTests()
    {
        _mockUserBookRepository = new Mock<IUserBookRepository>();
        _userBookService = new UserBookService(_mockUserBookRepository.Object);
    }

    [Fact]
    public async Task UpdateUserBookStatusAsync_ShouldUpdateStatusAndLoanStatus_WhenUserBookExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var currentReadingStatus = ReadingStatus.Reading;
        var currentLoanStatus = LoanStatus.Lent; // Corrected
        var newReadingStatus = ReadingStatus.Completed;
        var newLoanStatus = LoanStatus.None; // Corrected

        var command = new UpdateUserBookStatusCommand
        {
            UserId = userId,
            BookId = bookId,
            NewReadingStatus = newReadingStatus,
            NewLoanStatus = newLoanStatus
        };

        var existingUserBook = new UserBook(
            userId,
            bookId,
            true, // ownership
            currentReadingStatus,
            currentLoanStatus,
            DateTime.UtcNow,
            new Price(10.0m) // Corrected Price constructor
        );

        _mockUserBookRepository.Setup(r => r.GetAsync(userId, bookId))
            .ReturnsAsync(existingUserBook);
        _mockUserBookRepository.Setup(r => r.UpdateAsync(It.IsAny<UserBook>()))
            .ReturnsAsync((UserBook?)It.IsAny<UserBook>());

        // Act
        var result = await _userBookService.UpdateUserBookStatusAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newReadingStatus, result.ReadingStatus);
        Assert.Equal(newLoanStatus, result.LoanStatus);
        _mockUserBookRepository.Verify(r => r.GetAsync(userId, bookId), Times.Once);
        _mockUserBookRepository.Verify(r => r.UpdateAsync(
            It.Is<UserBook>(ub => ub.UserId == userId && ub.BookId == bookId && ub.ReadingStatus == newReadingStatus && ub.LoanStatus == newLoanStatus)), Times.Once);
    }

    [Fact]
    public async Task UpdateUserBookStatusAsync_ShouldReturnNull_WhenUserBookDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var newReadingStatus = ReadingStatus.Completed;

        var command = new UpdateUserBookStatusCommand
        {
            UserId = userId,
            BookId = bookId,
            NewReadingStatus = newReadingStatus
        };

        _mockUserBookRepository.Setup(r => r.GetAsync(userId, bookId))
            .ReturnsAsync((UserBook?)null);

        // Act
        var result = await _userBookService.UpdateUserBookStatusAsync(command);

        // Assert
        Assert.Null(result);
        _mockUserBookRepository.Verify(r => r.GetAsync(userId, bookId), Times.Once);
        _mockUserBookRepository.Verify(r => r.UpdateAsync(It.IsAny<UserBook>()), Times.Never);
    }

    [Fact]
    public async Task UpdateUserBookStatusAsync_ShouldOnlyUpdateReadingStatus_WhenLoanStatusIsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var bookId = Guid.NewGuid();
        var currentReadingStatus = ReadingStatus.Reading;
        var currentLoanStatus = LoanStatus.Lent; // Corrected
        var newReadingStatus = ReadingStatus.Completed;

        var command = new UpdateUserBookStatusCommand
        {
            UserId = userId,
            BookId = bookId,
            NewReadingStatus = newReadingStatus,
            NewLoanStatus = null // Only update reading status
        };

        var existingUserBook = new UserBook(
            userId,
            bookId,
            true, // ownership
            currentReadingStatus,
            currentLoanStatus,
            DateTime.UtcNow,
            new Price(10.0m) // Corrected Price constructor
        );

        _mockUserBookRepository.Setup(r => r.GetAsync(userId, bookId))
            .ReturnsAsync(existingUserBook);
        _mockUserBookRepository.Setup(r => r.UpdateAsync(It.IsAny<UserBook>()))
            .ReturnsAsync((UserBook?)It.IsAny<UserBook>());

        // Act
        var result = await _userBookService.UpdateUserBookStatusAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newReadingStatus, result.ReadingStatus);
        Assert.Equal(currentLoanStatus, result.LoanStatus); // Loan status should remain unchanged
        _mockUserBookRepository.Verify(r => r.GetAsync(userId, bookId), Times.Once);
        _mockUserBookRepository.Verify(r => r.UpdateAsync(
            It.Is<UserBook>(ub => ub.UserId == userId && ub.BookId == bookId && ub.ReadingStatus == newReadingStatus && ub.LoanStatus == currentLoanStatus)), Times.Once);
    }
}
