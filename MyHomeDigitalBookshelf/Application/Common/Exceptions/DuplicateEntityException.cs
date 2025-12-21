using System;

namespace MyHomeDigitalBookshelf.Application.Common.Exceptions;

public sealed class DuplicateEntityException : Exception
{
    public DuplicateEntityException(string message, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
