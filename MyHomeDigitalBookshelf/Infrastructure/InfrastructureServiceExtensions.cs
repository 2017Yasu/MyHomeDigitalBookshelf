using Microsoft.Extensions.DependencyInjection;
using MyHomeDigitalBookshelf.Utilities.Extensions;

namespace MyHomeDigitalBookshelf.Infrastructure;

public static class InfrastructureServiceExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, Database.DbSettings dbSettings)
    {
        return services.AddHttpClient().
            AddSingleton(dbSettings).
            AddAttributedServices(
                typeof(Api.GoogleBooksFinderService),
                typeof(Database.DbConnectionProvider),
                typeof(Database.Repositories.BookRepository),
                typeof(Database.Repositories.BookshelfRepository),
                typeof(Database.Repositories.CategoryRepository),
                typeof(Database.Repositories.BookshelfUserRepository),
                typeof(Database.Repositories.SessionRepository),
                typeof(Database.Repositories.UserBookRepository),
                typeof(Database.Repositories.UserIdentityRepository),
                typeof(Database.Repositories.UserRepository),
                typeof(Services.SmtpEmailService),
                typeof(Services.PasswordHasher),
                typeof(Services.TokenService)
        );
    }
}
