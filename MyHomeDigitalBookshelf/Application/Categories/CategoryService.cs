using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Domain.Repositories;
using MyHomeDigitalBookshelf.Domain.ValueObjects;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Application.Categories;

/// <summary>
/// Service for managing categories in the system.
/// </summary>
[SingletonService]
public class CategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBookshelfRepository _bookshelfRepository;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IBookshelfRepository bookshelfRepository)
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _bookshelfRepository = bookshelfRepository ?? throw new ArgumentNullException(nameof(bookshelfRepository));
    }

    /// <summary>
    /// Adds a new category to the system.
    /// </summary>
    /// <param name="command">The command containing category details.</param>
    /// <returns>The created category.</returns>
    /// <summary>
    /// Adds a new category to the system.
    /// </summary>
    /// <param name="command">The command containing category details.</param>
    /// <returns>The created category.</returns>
    public async Task<Category> AddCategoryAsync(Commands.AddCategoryCommand command)
    {
        command.Validate();

        // Validate that the bookshelf exists
        var bookshelf = await _bookshelfRepository.GetByIdAsync(command.BookshelfId);
        if (bookshelf == null)
        {
            throw new ArgumentException($"Bookshelf with ID {command.BookshelfId} not found.", nameof(command.BookshelfId));
        }

        var category = Category.CreateNew(
            command.Name,
            command.Description,
            command.BookshelfId);

        return await _categoryRepository.AddAsync(category);
    }

    /// <summary>
    /// Updates an existing category in the system.
    /// </summary>
    /// <param name="command">The command containing updated category details.</param>
    /// <returns>The updated category, or null if the category was not found.</returns>
    /// <summary>
    /// Updates an existing category in the system.
    /// </summary>
    /// <param name="command">The command containing updated category details.</param>
    /// <returns>The updated category, or null if the category was not found.</returns>
    public async Task<Category?> UpdateCategoryAsync(Commands.UpdateCategoryCommand command)
    {
        command.Validate();

        var existingCategory = await _categoryRepository.GetByIdAsync(command.Id);
        if (existingCategory == null)
        {
            return null;
        }

        // Validate that the bookshelf exists
        var bookshelf = await _bookshelfRepository.GetByIdAsync(command.BookshelfId);
        if (bookshelf == null)
        {
            throw new ArgumentException($"Bookshelf with ID {command.BookshelfId} not found.", nameof(command.BookshelfId));
        }

        var updatedCategory = new Category(
            command.Id,
            command.Name,
            command.Description,
            command.BookshelfId,
            existingCategory.CreatedAt,
            DateTime.UtcNow);

        return await _categoryRepository.UpdateAsync(updatedCategory);
    }

    /// <summary>
    /// Deletes a category from the system.
    /// </summary>
    /// <param name="command">The command containing the category ID to delete.</param>
    public async Task DeleteCategoryAsync(Commands.DeleteCategoryCommand command)
    {
        command.Validate();
        await _categoryRepository.DeleteAsync(command.Id);
    }

    /// <summary>
    /// Gets a category by its ID.
    /// </summary>
    /// <param name="query">The query containing the category ID.</param>
    /// <returns>The category if found; otherwise, null.</returns>
    public async Task<Category?> GetCategoryByIdAsync(Queries.GetCategoryByIdQuery query)
    {
        query.Validate();
        return await _categoryRepository.GetByIdAsync(query.Id);
    }

    /// <summary>
    /// Gets all categories for a specific bookshelf.
    /// </summary>
    /// <param name="query">The query containing the bookshelf ID.</param>
    /// <returns>An array of categories for the bookshelf.</returns>
    public async Task<Category[]> GetCategoriesByBookshelfAsync(Queries.GetCategoriesByBookshelfQuery query)
    {
        query.Validate();

        // Validate that the bookshelf exists
        var bookshelf = await _bookshelfRepository.GetByIdAsync(query.BookshelfId);
        if (bookshelf == null)
        {
            throw new ArgumentException($"Bookshelf with ID {query.BookshelfId} not found.", nameof(query.BookshelfId));
        }

        return await _categoryRepository.GetAllByBookshelfAsync(query.BookshelfId);
    }

    /// <summary>
    /// Suggests a category for a book based on its CCode.
    /// </summary>
    /// <param name="bookshelfId">The ID of the bookshelf to search categories in.</param>
    /// <param name="cCode">The CCode of the book.</param>
    /// <returns>The suggested category or null if no match is found.</returns>
    /// <remarks>
    /// This method attempts to find a category that best matches the book's CCode classification.
    /// It looks at the genre part of the CCode first, then audience, and finally format if needed.
    /// </remarks>
    public async Task<Category?> SuggestCategoryAsync(Guid bookshelfId, CCode cCode)
    {
        if (bookshelfId == Guid.Empty)
        {
            throw new ArgumentException("BookshelfId is required.", nameof(bookshelfId));
        }

        var categories = await _categoryRepository.GetAllByBookshelfAsync(bookshelfId);
        if (categories.Length == 0)
        {
            return null;
        }

        // Convert the CCode to a general category name based on the genre codes
        string suggestedCategoryName = cCode.GetGenre() switch
        {
            // Generalities, Encyclopedias, etc.
            var g when g >= 0 && g <= 4 => "Reference",

            // Philosophy, Psychology, Religion
            var g when g >= 10 && g <= 16 => "Philosophy & Religion",

            // History, Biography, Geography
            var g when g >= 20 && g <= 26 => "History & Biography",

            // Social Sciences, Law, Economics, Education
            var g when g >= 30 && g <= 39 => "Social Sciences",

            // Natural Sciences, Mathematics, Medicine
            var g when g >= 40 && g <= 47 => "Science & Mathematics",

            // Engineering
            var g when g >= 50 && g <= 58 => "Technology & Engineering",

            // Agriculture, Fisheries, Commerce, Transport
            var g when g >= 60 && g <= 65 => "Business & Industry",

            // Arts, Sports, Entertainment
            var g when g >= 70 && g <= 79 => "Arts & Entertainment",

            // Languages
            var g when g >= 80 && g <= 87 => "Languages",

            // Literature
            var g when g >= 90 && g <= 98 => "Literature",

            // Default
            _ => "General"
        };

        // Look for an exact match first
        var category = categories.FirstOrDefault(c => c.Name.Equals(suggestedCategoryName, StringComparison.OrdinalIgnoreCase));
        if (category != null)
        {
            return category;
        }

        // Look for a partial match
        category = categories.FirstOrDefault(c => c.Name.Contains(suggestedCategoryName, StringComparison.OrdinalIgnoreCase));
        if (category != null)
        {
            return category;
        }

        // If no match is found, return the general category if it exists, otherwise null
        return categories.FirstOrDefault(c => c.Name.Equals("General", StringComparison.OrdinalIgnoreCase));
    }
}
