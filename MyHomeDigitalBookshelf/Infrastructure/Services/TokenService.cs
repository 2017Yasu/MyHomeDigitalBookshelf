using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyHomeDigitalBookshelf.Application.Common.Interfaces;
using MyHomeDigitalBookshelf.Domain.Entities;
using MyHomeDigitalBookshelf.Utilities.Attributes.Registration;

namespace MyHomeDigitalBookshelf.Infrastructure.Services;

[SingletonService]
public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private const string Secret = "your-super-secret-key-that-should-be-at-least-32-characters-long-for-security";
    private const string Issuer = "MyHomeDigitalBookshelf";
    private const string Audience = "MyHomeDigitalBookshelfUsers";
    private const int ExpirationMinutes = 60;

    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public string CreateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        try
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email?.Value ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(ExpirationMinutes),
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var encodedToken = tokenHandler.WriteToken(token);

            _logger.LogInformation("Token created successfully for user {UserId}", user.Id);

            return encodedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create token for user {UserId}", user.Id);
            throw;
        }
    }
}
