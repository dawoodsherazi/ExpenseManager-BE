using AutoMapper;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Mapper
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Book, BookDto>();
            CreateMap<BookDto, Book>();

            CreateMap<Category, CategoryDto>(); 
            CreateMap<CategoryDto, Category>();

            CreateMap<Transaction, TransactionDto>();
            CreateMap<TransactionDto, Transaction>();
        }

    }
}
