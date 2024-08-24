using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;

namespace LibraryDataAccess.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryContext _context;

        public AuthorRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Author> CreateAuthorAsync(Author author)
        {
            var createdAuthor = await _context.AddAsync(author);
            await _context.SaveChangesAsync();
            return createdAuthor.Entity;
        }

        public async Task DeleteAuthorAsync(int id)
        {

            var authorToDelete = await _context.Authors.FirstOrDefaultAsync(c => c.AuthorId == id);
            if (authorToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Authors.Remove(authorToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Author?> GetAuthorByIdAsync(int id)
        {
            return await _context.Authors.FirstOrDefaultAsync(c => c.AuthorId == id);
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            return await _context.Authors.ToListAsync();
        }

        public async Task<Author> UpdateAuthorAsync(Author author)
        {
            var authorToUpdate = await _context.Authors.FirstOrDefaultAsync(c => c.AuthorId == author.AuthorId);
            if (authorToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            authorToUpdate.AuthorName = author.AuthorName;
            var authorUpdated = _context.Update(authorToUpdate);
            await _context.SaveChangesAsync();
            return authorUpdated.Entity;
        }
    }
}
