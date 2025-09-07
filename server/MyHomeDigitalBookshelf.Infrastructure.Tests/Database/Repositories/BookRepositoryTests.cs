using System;
using MyHomeDigitalBookshelf.Infrastructure.Database.Repositories;
using Xunit.Abstractions;

namespace MyHomeDigitalBookshelf.Infrastructure.Tests.Database.Repositories;

public class BookRepositoryTests : RepositoryTestBase
{
    private readonly BookRepository _repository;

    public BookRepositoryTests(ITestOutputHelper outputHelper) : base(outputHelper)
    {
        _repository = new BookRepository(CreateLogger<BookRepository>(), GetConnectionProvider());
    }
}
