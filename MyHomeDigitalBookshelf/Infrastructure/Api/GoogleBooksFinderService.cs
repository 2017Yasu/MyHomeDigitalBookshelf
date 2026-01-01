using System.Text.Json;
using MyHomeDigitalBookshelf.Application.Books.Interfaces;
using MyHomeDigitalBookshelf.Domain.Entities;
using System.Net.Http; // Added

namespace MyHomeDigitalBookshelf.Infrastructure.Api;

public class GoogleBooksFinderService : IBookFinderService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string ApiKey = "YOUR_GOOGLE_BOOKS_API_KEY"; // This should be loaded from a secure configuration

    public GoogleBooksFinderService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Book?> FindByIsbnAsync(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
        {
            return null;
        }

        var client = _httpClientFactory.CreateClient("GoogleBooks");
        var url = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}&key={ApiKey}";

        try
        {
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                // Log error
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            
            // This is a simplified parsing logic. A real implementation would use a proper DTO.
            using var doc = JsonDocument.Parse(content);
            var firstItem = doc.RootElement.GetProperty("items")[0];
            var volumeInfo = firstItem.GetProperty("volumeInfo");

            var title = volumeInfo.GetProperty("title").GetString() ?? "Unknown Title";
            var authors = volumeInfo.TryGetProperty("authors", out var authorsProp) 
                ? authorsProp.EnumerateArray().Select(a => a.GetString() ?? "").ToArray() 
                : new string[0];
            
            // This is a placeholder for creating a full Book entity.
            // In a real scenario, you would map all the required fields.
            // The bookshelfId is also a placeholder and would need to be provided.
            var book = Book.CreateNew(title, Guid.NewGuid(), authors: authors);

            return book;
        }
        catch (Exception ex)
        {
            // Log exception
            return null;
        }
    }
}
