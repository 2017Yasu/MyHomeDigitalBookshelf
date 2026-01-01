using Microsoft.Extensions.DependencyInjection;
using MyHomeDigitalBookshelf.Utilities.Extensions;

namespace MyHomeDigitalBookshelf.Application;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        return services.AddAttributedServices(
            typeof(Books.BookService),
            typeof(Bookshelves.BookshelfService),
            typeof(Categories.CategoryService),
            typeof(Sessions.SessionService),
            typeof(UserBooks.UserBookService),
            typeof(Users.UserService)
        );
    }
}
