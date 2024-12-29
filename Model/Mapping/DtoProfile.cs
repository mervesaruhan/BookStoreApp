using AutoMapper;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.DTO.UserDtos;
using BookStoreApp.Model.DTO.BookDtos;
using BookStoreApp.Model.DTO.OrderDtos;
using BookStoreApp.Model.DTO.CategoryDtos;
using BookStoreApp.Model.DTO.PaymentDtos;


namespace BookStoreApp.Model.Mapping
{
    public class DtoProfile:Profile
    {
        public DtoProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<UserRegisterDto, User>().ReverseMap();


            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<BookCreateDto, BookDto>().ReverseMap();
            CreateMap<UpdateBookDto, BookDto>().ReverseMap();
            CreateMap<BookCreateDto, Book>().ReverseMap();


            CreateMap<OrderCreateDto , OrderDto>().ReverseMap();
            CreateMap<Order,OrderDto>().ReverseMap();
            CreateMap<OrderCreateDto, Order>().ReverseMap();



            CreateMap<OrderItemCreateDto, OrderItem>();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();

            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryCreateDto , Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
            CreateMap<CategoryListDto, Category>().ReverseMap();


            CreateMap<PaymentCreateDto, Payment>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();

        }


    }
}
