using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests;

internal sealed class XunitLogger(ITestOutputHelper outputHelper, string categoryName) : ILogger
{
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return NopDisposable.Instance;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        outputHelper.WriteLine($"{categoryName} [{eventId}] {formatter(state, exception)}");

        if (exception is not null)
        {
            outputHelper.WriteLine(exception.ToString());
        }
    }

    private class NopDisposable : IDisposable
    {
        public static NopDisposable Instance = new();
        public void Dispose() { }
    }
}
