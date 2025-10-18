using Moq;
using MyHomeDigitalBookshelf.Application.Categories.Commands;
using MyHomeDigitalBookshelf.Application.Categories.Queries;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects;

namespace MyHomeDigitalBookshelf.Application.Tests.Categories;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly Mock<IBookshelfRepository> _mockBookshelfRepository;
    private readonly Application.Categories.CategoryService _service;

    public CategoryServiceTests()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _mockBookshelfRepository = new Mock<IBookshelfRepository>();
        _service = new Application.Categories.CategoryService(_mockCategoryRepository.Object, _mockBookshelfRepository.Object);
    }

    [Fact]
    public async Task AddCategory_WithValidCommand_ReturnsCreatedCategory()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var command = new AddCategoryCommand(
            Name: "Test Category",
            BookshelfId: bookshelfId,
            Description: "Test description"
        );

        var bookshelf = new Bookshelf(
            bookshelfId,
            "Test Bookshelf",
            null,
            DateTime.UtcNow,
            DateTime.UtcNow);

        var expectedCategory = Category.CreateNew(
            command.Name,
            command.Description,
            command.BookshelfId);

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync(bookshelf);

        _mockCategoryRepository.Setup(r => r.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category category) => category);

        // Act
        var result = await _service.AddCategoryAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.BookshelfId, result.BookshelfId);
        Assert.Equal(command.Description, result.Description);

        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
        _mockCategoryRepository.Verify(r => r.AddAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task AddCategory_WithInvalidBookshelf_ThrowsArgumentException()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var command = new AddCategoryCommand(
            Name: "Test Category",
            BookshelfId: bookshelfId);

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync((Bookshelf?)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _service.AddCategoryAsync(command));
    }

    [Fact]
    public async Task GetCategoriesByBookshelf_WithValidQuery_ReturnsCategories()
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var query = new GetCategoriesByBookshelfQuery(bookshelfId);

        var bookshelf = new Bookshelf(
            bookshelfId,
            "Test Bookshelf",
            null,
            DateTime.UtcNow,
            DateTime.UtcNow);

        var expectedCategories = new[]
        {
            Category.CreateNew("Category 1", null, bookshelfId),
            Category.CreateNew("Category 2", null, bookshelfId)
        };

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync(bookshelf);

        _mockCategoryRepository.Setup(r => r.GetAllByBookshelfAsync(bookshelfId))
            .ReturnsAsync(expectedCategories);

        // Act
        var result = await _service.GetCategoriesByBookshelfAsync(query);

        // Assert
        Assert.Equal(expectedCategories.Length, result.Length);
        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
        _mockCategoryRepository.Verify(r => r.GetAllByBookshelfAsync(bookshelfId), Times.Once);
    }

    [Fact]
    public async Task GetCategoryById_WithExistingCategory_ReturnsCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryByIdQuery(categoryId);

        var expectedCategory = Category.CreateNew("Test Category", null, Guid.NewGuid());

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(expectedCategory);

        // Act
        var result = await _service.GetCategoryByIdAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategory.Name, result.Name);
        _mockCategoryRepository.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task UpdateCategory_WithValidCommand_ReturnsUpdatedCategory()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var bookshelfId = Guid.NewGuid();
        var command = new UpdateCategoryCommand(
            Id: categoryId,
            Name: "Updated Category",
            BookshelfId: bookshelfId,
            Description: "Updated description"
        );

        var bookshelf = new Bookshelf(
            bookshelfId,
            "Test Bookshelf",
            null,
            DateTime.UtcNow,
            DateTime.UtcNow);

        var existingCategory = new Category(
            categoryId,
            "Original Category",
            "Original description",
            bookshelfId,
            DateTime.UtcNow.AddDays(-1),
            DateTime.UtcNow.AddDays(-1));

        _mockBookshelfRepository.Setup(r => r.GetByIdAsync(bookshelfId))
            .ReturnsAsync(bookshelf);

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        _mockCategoryRepository.Setup(r => r.UpdateAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category category) => category);

        // Act
        var result = await _service.UpdateCategoryAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        Assert.Equal(command.Description, result.Description);

        _mockBookshelfRepository.Verify(r => r.GetByIdAsync(bookshelfId), Times.Once);
        _mockCategoryRepository.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
        _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Once);
    }

    [Fact]
    public async Task UpdateCategory_WithNonExistingCategory_ReturnsNull()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new UpdateCategoryCommand(
            Id: categoryId,
            Name: "Updated Category",
            BookshelfId: Guid.NewGuid());

        _mockCategoryRepository.Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act
        var result = await _service.UpdateCategoryAsync(command);

        // Assert
        Assert.Null(result);
        _mockCategoryRepository.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
        _mockCategoryRepository.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }

    [Fact]
    public async Task DeleteCategory_WithValidCommand_CallsDeleteAsync()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand(categoryId);

        _mockCategoryRepository.Setup(r => r.DeleteAsync(categoryId))
            .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteCategoryAsync(command);

        // Assert
        _mockCategoryRepository.Verify(r => r.DeleteAsync(categoryId), Times.Once);
    }

    [Theory]
    [InlineData("0093", "Literature")] // Japanese Novels
    [InlineData("0037", "Social Sciences")] // Education
    [InlineData("0040", "Science & Mathematics")] // Natural Sciences
    [InlineData("0070", "Arts & Entertainment")] // Arts
    public async Task SuggestCategory_WithValidCCode_ReturnsSuggestedCategory(string cCodeValue, string expectedCategoryName)
    {
        // Arrange
        var bookshelfId = Guid.NewGuid();
        var cCode = new CCode(cCodeValue);
        var categories = new[]
        {
            Category.CreateNew("Literature", null, bookshelfId),
            Category.CreateNew("Social Sciences", null, bookshelfId),
            Category.CreateNew("Science & Mathematics", null, bookshelfId),
            Category.CreateNew("Arts & Entertainment", null, bookshelfId),
            Category.CreateNew("General", null, bookshelfId)
        };

        _mockCategoryRepository.Setup(r => r.GetAllByBookshelfAsync(bookshelfId))
            .ReturnsAsync(categories);

        // Act
        var result = await _service.SuggestCategoryAsync(bookshelfId, cCode);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedCategoryName, result.Name);
        _mockCategoryRepository.Verify(r => r.GetAllByBookshelfAsync(bookshelfId), Times.Once);
    }
}
