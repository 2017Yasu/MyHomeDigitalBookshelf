using System;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class BookshelfRepositoryTests : RepositoryTestBase
{
    private readonly BookshelfRepository _repository;

    public BookshelfRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repository = new BookshelfRepository(CreateLogger<BookshelfRepository>(), GetConnectionProvider());
    }

    [Fact(DisplayName = "AddAsync should add a bookshelf and return the created entity")]
    public async Task AddAsync_ShouldAddBookshelfAndReturnCreated()
    {
        // Arrange
        var bookshelf = Bookshelf.CreateNew("Test Bookshelf", "A bookshelf for testing");

        // Act
        var newBookshelf = await _repository.AddAsync(bookshelf);

        // Assert
        Assert.NotNull(newBookshelf);
        Assert.Equal(bookshelf.Name, newBookshelf.Name);
        Assert.Equal(bookshelf.Description, newBookshelf.Description);

        // Act
        var fetchedBookshelf = await _repository.GetByIdAsync(newBookshelf.Id);

        // Assert
        Assert.NotNull(fetchedBookshelf);
        Assert.Equal(newBookshelf.Id, fetchedBookshelf.Id);
    }

    [Fact(DisplayName = "GetByIdAsync should return null when the bookshelf does not exist")]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        // Arrange
        var id = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(id);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetAllAsync should return all bookshelves")]
    public async Task GetAllAsync_ShouldReturnAllBookshelves()
    {
        // Arrange
        var bookshelves = new[]
        {
            Bookshelf.CreateNew("Bookshelf 1", "First bookshelf"),
            Bookshelf.CreateNew("Bookshelf 2", "Second bookshelf"),
            Bookshelf.CreateNew("Bookshelf 3", "Third bookshelf"),
        };

        // Act
        foreach (var shelf in bookshelves)
        {
            var created = await _repository.AddAsync(shelf);
        }
        var allBookshelves = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(allBookshelves);
        Assert.All(bookshelves, shelf => Assert.Contains(allBookshelves, x => x.Name == shelf.Name && x.Description == shelf.Description));
    }

    [Fact(DisplayName = "UpdateAsync should update a bookshelf and return the updated entity")]
    public async Task UpdateAsync_ShouldUpdateBookshelfAndReturnUpdated()
    {
        // Arrange
        var bookshelf = Bookshelf.CreateNew("Initial Name", "Initial Description");
        var addedBookshelf = await _repository.AddAsync(bookshelf);
        var updatedBookshelf = addedBookshelf.Update("Updated Name", "Updated Description");

        // Act
        var result = await _repository.UpdateAsync(updatedBookshelf);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedBookshelf.Id, result.Id);
        Assert.Equal(updatedBookshelf.Name, result.Name);
        Assert.Equal(updatedBookshelf.Description, result.Description);
    }

    [Fact(DisplayName = "DeleteAsync should remove the bookshelf")]
    public async Task DeleteAsync_ShouldRemoveBookshelf()
    {
        // Arrange
        var bookshelf = Bookshelf.CreateNew("To Be Deleted", "This bookshelf will be deleted");
        var addedBookshelf = await _repository.AddAsync(bookshelf);

        // Act
        await _repository.DeleteAsync(addedBookshelf.Id);
        var deletedBookshelf = await _repository.GetByIdAsync(addedBookshelf.Id);

        // Assert
        Assert.Null(deletedBookshelf);
    }
}
