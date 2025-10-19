using Microsoft.Extensions.Logging;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class CategoryRepositoryTests : RepositoryTestBase
{
    private readonly CategoryRepository _repository;

    public CategoryRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repository = new CategoryRepository(CreateLogger<CategoryRepository>(), GetConnectionProvider());
    }

    [Fact(DisplayName = "AddAsync should add a category and return the created entity")]
    public async Task AddAsync_ShouldAddCategoryAndReturnCreated()
    {
        // Arrange
        var bookshelf = await CreateTestBookshelf();
        var category = Category.CreateNew("Test Category", "A category for testing", bookshelf.Id);

        // Act
        var newCategory = await _repository.AddAsync(category);

        // Assert
        Assert.NotNull(newCategory);
        Assert.NotEqual(Guid.Empty, newCategory.Id);
        Assert.Equal(category.Name, newCategory.Name);
        Assert.Equal(category.Description, newCategory.Description);
        Assert.Equal(category.BookshelfId, newCategory.BookshelfId);

        // Act - Verify retrieval
        var fetchedCategory = await _repository.GetByIdAsync(newCategory.Id);

        // Assert
        Assert.NotNull(fetchedCategory);
        Assert.Equal(newCategory.Id, fetchedCategory.Id);
        Assert.Equal(newCategory.Name, fetchedCategory.Name);
        Assert.Equal(newCategory.Description, fetchedCategory.Description);
        Assert.Equal(newCategory.BookshelfId, fetchedCategory.BookshelfId);
    }

    [Fact(DisplayName = "GetByIdAsync should return null when category does not exist")]
    public async Task GetByIdAsync_ShouldReturnNullForNonExistentCategory()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var result = await _repository.GetByIdAsync(nonExistentId);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "GetAllByBookshelfAsync should return all categories for a bookshelf")]
    public async Task GetAllByBookshelfAsync_ShouldReturnAllCategoriesForBookshelf()
    {
        // Arrange
        var bookshelf = await CreateTestBookshelf();
        var categories = new[]
        {
            Category.CreateNew("Fiction", "Fiction books", bookshelf.Id),
            Category.CreateNew("Non-Fiction", "Non-fiction books", bookshelf.Id),
            Category.CreateNew("Comics", "Comic books and manga", bookshelf.Id)
        };

        foreach (var category in categories)
        {
            await _repository.AddAsync(category);
        }

        // Create a category in another bookshelf to ensure isolation
        var otherBookshelf = await CreateTestBookshelf();
        await _repository.AddAsync(Category.CreateNew("Other", "Category in another bookshelf", otherBookshelf.Id));

        // Act
        var result = await _repository.GetAllByBookshelfAsync(bookshelf.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categories.Length, result.Length);
        Assert.All(result, c => Assert.Equal(bookshelf.Id, c.BookshelfId));
        Assert.Contains(result, c => c.Name == "Fiction");
        Assert.Contains(result, c => c.Name == "Non-Fiction");
        Assert.Contains(result, c => c.Name == "Comics");
    }

    [Fact(DisplayName = "UpdateAsync should update category and return the updated entity")]
    public async Task UpdateAsync_ShouldUpdateCategoryAndReturnUpdated()
    {
        // Arrange
        var bookshelf = await CreateTestBookshelf();
        var category = await _repository.AddAsync(
            Category.CreateNew("Original Name", "Original description", bookshelf.Id));

        // Update the category
        var updatedCategory = new Category(
            category.Id,
            "Updated Name",
            "Updated description",
            category.BookshelfId,
            category.CreatedAt,
            category.UpdatedAt);

        // Act
        var result = await _repository.UpdateAsync(updatedCategory);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(category.Id, result.Id);
        Assert.Equal("Updated Name", result.Name);
        Assert.Equal("Updated description", result.Description);
        Assert.Equal(category.BookshelfId, result.BookshelfId);
        Assert.Equal(category.CreatedAt, result.CreatedAt);
        Assert.NotEqual(category.UpdatedAt, result.UpdatedAt);

        // Verify the update with a fresh fetch
        var fetchedCategory = await _repository.GetByIdAsync(category.Id);
        Assert.NotNull(fetchedCategory);
        Assert.Equal("Updated Name", fetchedCategory.Name);
        Assert.Equal("Updated description", fetchedCategory.Description);
    }

    [Fact(DisplayName = "UpdateAsync should return null when category does not exist")]
    public async Task UpdateAsync_ShouldReturnNullForNonExistentCategory()
    {
        // Arrange
        var bookshelf = await CreateTestBookshelf();
        var nonExistentCategory = new Category(
            Guid.NewGuid(),
            "Non-existent",
            "This category does not exist",
            bookshelf.Id,
            DateTime.UtcNow,
            DateTime.UtcNow);

        // Act
        var result = await _repository.UpdateAsync(nonExistentCategory);

        // Assert
        Assert.Null(result);
    }

    [Fact(DisplayName = "DeleteAsync should delete the category")]
    public async Task DeleteAsync_ShouldDeleteCategory()
    {
        // Arrange
        var bookshelf = await CreateTestBookshelf();
        var category = await _repository.AddAsync(
            Category.CreateNew("Test Category", "A category to delete", bookshelf.Id));

        // Act
        await _repository.DeleteAsync(category.Id);

        // Assert
        var result = await _repository.GetByIdAsync(category.Id);
        Assert.Null(result);
    }

    private async Task<Bookshelf> CreateTestBookshelf()
    {
        var bookshelfRepo = new BookshelfRepository(CreateLogger<BookshelfRepository>(), GetConnectionProvider());
        return await bookshelfRepo.AddAsync(
            Bookshelf.CreateNew("Test Bookshelf", "A bookshelf for testing"));
    }
}
