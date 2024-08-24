using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryDataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Book> CreateBookAsync(Book book)
        {
            var bookCreated = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return bookCreated.Entity;
        }

        public async Task DeleteBookAsync(int id)
        {
            var bookToDelete = await _context.Books.FirstOrDefaultAsync(c => c.BookId == id);
            if (bookToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(c => c.BookId == book.BookId);
            if (bookToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            bookToUpdate.AuthorId = book.AuthorId;
            bookToUpdate.CategoryId = book.CategoryId;
            bookToUpdate.Title = book.Title;
            bookToUpdate.Price = book.Price;
            var updatedBook = _context.Update(bookToUpdate);
            await _context.SaveChangesAsync();
            return updatedBook.Entity;
        }
    }
}
