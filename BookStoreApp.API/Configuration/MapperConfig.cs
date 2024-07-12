using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.Models.Author;
using BookStoreApp.Models.Book;
using BookStoreApp.Models.User;

namespace BookStoreApp.API.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<AuthorCreateDto, Author>().ReverseMap();
            CreateMap<BookCreateDto, Book>().ReverseMap();
            CreateMap<BookUpdateDto, Book>().ReverseMap();
            CreateMap<Book, BookReadOnlyDto>()
                .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();

            CreateMap<Book, BookDetailsDto>()
                .ForMember(q => q.AuthorName, d => d.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
                .ReverseMap();

            CreateMap<ApiUser, UserDto>().ReverseMap();


        }
    }
}
