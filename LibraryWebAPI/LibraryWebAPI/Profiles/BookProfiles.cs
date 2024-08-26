using AutoMapper;
using LibraryDataAccess.Models;
using LibraryWebAPI.Dtos.Books;
using Libray.Core;

namespace LibraryWebAPI.Profiles
{
    public class BookProfiles : Profile
    {
        public BookProfiles()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<CreateBookDto, Book>().ReverseMap();
            CreateMap<PaginatedList<Book>, PaginatedList<BookDto>>();
        }
    }
}
