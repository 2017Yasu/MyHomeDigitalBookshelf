using MyHomeDigitalBookshelf.Domain.Entities;

namespace MyHomeDigitalBookshelf.Application.Common.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
