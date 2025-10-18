using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Users.Queries;

/// <summary>
/// Query to get a user identity by provider and subject.
/// </summary>
public record GetUserIdentityByProviderQuery(string Provider, string Subject)
{
    /// <summary>
    /// Validates the query parameters.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when required parameters are invalid.</exception>
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Provider))
        {
            throw new ArgumentException("Provider is required.", nameof(Provider));
        }
        if (string.IsNullOrWhiteSpace(Subject))
        {
            throw new ArgumentException("Subject is required.", nameof(Subject));
        }
    }
}
