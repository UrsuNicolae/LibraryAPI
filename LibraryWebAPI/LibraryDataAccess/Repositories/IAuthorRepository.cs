using LibraryDataAccess.Models;

namespace LibraryDataAccess.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAuthorsAsync();

        Task<Author?> GetAuthorByIdAsync(int id);

        Task<Author> CreateAuthorAsync(Author author);

        Task<Author> UpdateAuthorAsync(Author author);

        Task DeleteAuthorAsync(int id);
    }
}
