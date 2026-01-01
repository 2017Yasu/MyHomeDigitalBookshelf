using Moq;
using MyHomeDigitalBookshelf.Application.UserBooks.Commands;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using Xunit;
using System;
using System.Threading.Tasks;
using MyHomeDigitalBookshelf.Application.UserBooks; // Assuming a UserBookService exists

namespace MyHomeDigitalBookshelf.Application.Tests.UserBooks;

public class UserBookServiceTests
{
    private readonly Mock<IUserBookRepository> _mockUserBookRepository;
    private readonly UserBookService _userBookService; // Assuming a UserBookService

    public UserBookServiceTests()
    {
        _mockUserBookRepository = new Mock<IUserBookRepository>();
        // Initialize UserBookService with its dependencies.
        // If UserBookService has more dependencies, they would need to be mocked here too.
        _userBookService = new UserBookService(_mockUserBookRepository.Object);
    }

    [Fact]
    public async Task UpdateUserBookStatusAsync_ShouldUpdateStatus_WhenUserBookExists()
    {
        // Arrange
        var userBookId = Guid.NewGuid();
        var currentReadingStatus = ReadingStatus.Reading;
        var newReadingStatus = ReadingStatus.Completed;

        var command = new UpdateUserBookStatusCommand
        {
            UserBookId = userBookId,
            NewReadingStatus = newReadingStatus
        };

        var existingUserBook = new UserBook(
            userBookId,
            Guid.NewGuid(), // UserId
            Guid.NewGuid(), // BookId
            currentReadingStatus,
            null, null, null, null,
            DateTime.UtcNow,
            DateTime.UtcNow
        );

        _mockUserBookRepository.Setup(r => r.GetByIdAsync(userBookId))
            .ReturnsAsync(existingUserBook);
        _mockUserBookRepository.Setup(r => r.UpdateAsync(It.IsAny<UserBook>()))
            .Returns(Task.CompletedTask);

        // Act
        await _userBookService.UpdateUserBookStatusAsync(command);

        // Assert
        _mockUserBookRepository.Verify(r => r.GetByIdAsync(userBookId), Times.Once);
        _mockUserBookRepository.Verify(r => r.UpdateAsync(
            It.Is<UserBook>(ub => ub.Id == userBookId && ub.ReadingStatus == newReadingStatus)), Times.Once);
    }

    [Fact]
    public async Task UpdateUserBookStatusAsync_ShouldThrowException_WhenUserBookDoesNotExist()
    {
        // Arrange
        var userBookId = Guid.NewGuid();
        var newReadingStatus = ReadingStatus.Completed;

        var command = new UpdateUserBookStatusCommand
        {
            UserBookId = userBookId,
            NewReadingStatus = newReadingStatus
        };

        _mockUserBookRepository.Setup(r => r.GetByIdAsync(userBookId))
            .ReturnsAsync((UserBook?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            _userBookService.UpdateUserBookStatusAsync(command));

        _mockUserBookRepository.Verify(r => r.GetByIdAsync(userBookId), Times.Once);
        _mockUserBookRepository.Verify(r => r.UpdateAsync(It.IsAny<UserBook>()), Times.Never);
    }
}
