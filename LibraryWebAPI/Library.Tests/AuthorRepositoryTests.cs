using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class AuthorRepositoryTests
    {
        private List<Author> _categories = new List<Author>
        {
            new Author {AuthorId = 1, AuthorName = "FirstAuthor"},
            new Author {AuthorId = 2, AuthorName = "SecondAuthor"},
            new Author {AuthorId = 3, AuthorName = "ThirdAuthor"},
            new Author {AuthorId = 4, AuthorName = "ForthAuthor"},
        };


        [Fact]
        public async Task AuthorRepositoryGetAllAuthors_ShouldReturnZeroAuthors_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(!context.Authors.Any());
            var categories = await repo.GetAuthorsAsync();
            Assert.True(categories.Count == 0);
        }

        [Fact]
        public async Task AuthorRepositoryGetAllAuthors_ShouldReturnAuthors_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddRangeAsync(_categories);
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(context.Authors.Any());
            var categories = await repo.GetAuthorsAsync();
            Assert.True(categories.Any());
            Assert.True(categories.Count == _categories.Count);
            Assert.Equal(_categories, categories);
        }

        [Fact]
        public async Task AuthorRepositoryGetAuthorById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(!context.Authors.Any());
            var author = await repo.GetAuthorByIdAsync(1);
            Assert.Null(author);
        }

        [Fact]
        public async Task AuthorRepositoryGetAuthorById_ShouldReturnAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddAsync(_categories.ElementAt(0));
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.True(context.Authors.Any());
            var category = await repo.GetAuthorByIdAsync(_categories.ElementAt(0).AuthorId);
            Assert.NotNull(category);
            Assert.Equal(_categories.ElementAt(0), category);
        }

        [Fact]
        public async Task AuthorRepositoryCreateAuthor_ShouldCreateAuthor_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);


            Assert.Null(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _categories.ElementAt(0).AuthorId));
            var category = await repo.CreateAuthorAsync(_categories.ElementAt(0));
            Assert.NotNull(category);
            Assert.Equal(_categories.ElementAt(0), category);
        }

        [Fact]
        public async Task AuthorRepositoryDeleteAuthor_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);


            Assert.Null(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _categories.ElementAt(0).AuthorId));
           
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_categories.ElementAt(0).AuthorId));
        }

        [Fact]
        public async Task AuthorRepositoryDeleteAuthor_ShouldRemoveAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddAsync(_categories.ElementAt(0));
            await context.SaveChangesAsync();
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.NotNull(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _categories.ElementAt(0).AuthorId));
            await repo.DeleteAuthorAsync(_categories.ElementAt(0).AuthorId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteAuthorAsync(_categories.ElementAt(0).AuthorId));
        }


        [Fact]
        public async Task AuthorRepositoryUpdateAuthor_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            IAuthorRepository repo = new AuthorRepository(context);


            Assert.Null(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _categories.ElementAt(0).AuthorId));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateAuthorAsync(_categories.ElementAt(0)));
        }

        [Fact]
        public async Task AuthorRepositoryUpdateAuthor_ShouldUpdateAuthor_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Authors.AddAsync(_categories.ElementAt(0));
            await context.SaveChangesAsync();

            var categoryToUpdate = new Author
            {
                AuthorName = "UpdatedName",
                AuthorId = _categories.ElementAt(0).AuthorId
            };
            IAuthorRepository repo = new AuthorRepository(context);

            Assert.NotNull(await context.Authors.FirstOrDefaultAsync(c => c.AuthorId == _categories.ElementAt(0).AuthorId));

            var categoryUpdated = await repo.UpdateAuthorAsync(categoryToUpdate);
            Assert.NotSame(categoryToUpdate, categoryUpdated);
            Assert.Equal(categoryToUpdate.AuthorName, categoryUpdated.AuthorName);
        }
    }
}