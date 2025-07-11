using System.Reflection;
using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.Models;
namespace LibraryManagement.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterRequestDto, User>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();


        }
    }
}
