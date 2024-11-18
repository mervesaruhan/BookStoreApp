using AutoMapper;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.DTO;

namespace BookStoreApp.Model.Mapping
{
    public class DtoProfile:Profile
    {
        public DtoProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>();

        }


    }
}
