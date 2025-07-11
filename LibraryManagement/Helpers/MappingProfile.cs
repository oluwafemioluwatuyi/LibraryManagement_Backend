﻿using AutoMapper;
using LibraryManagement.DTOs;
using LibraryManagement.Models;
namespace LibraryManagement.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<RegisterRequestDto, User>();
            CreateMap<CreateBookDto, Book>();
            CreateMap<UpdateBookDto, Book>();
            CreateMap<Book, BookDto>();

        }
    }
}
