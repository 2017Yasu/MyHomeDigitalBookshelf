using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests;

internal sealed class XunitLoggerProvider(ITestOutputHelper outputHelper) : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new XunitLogger(outputHelper, categoryName);
    }

    public void Dispose()
    {
    }
}
