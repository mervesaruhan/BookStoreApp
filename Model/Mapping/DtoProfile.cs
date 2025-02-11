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


            //Book Mapping
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.CategoryId).ToList())) // Book → DTO dönüşümünde CategoryIds kullan
                .ReverseMap();

            CreateMap<BookCreateDto, Book>()
                .ForMember(dest => dest.BookCategories, opt => opt.MapFrom<BookCategoryResolver>()); // DTO → Entity dönüşümünde CategoryIds → BookCategories olarak maplenecek

            CreateMap<UpdateBookDto, Book>().ReverseMap();


            CreateMap<OrderCreateDto, OrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<OrderCreateDto, Order>().ReverseMap();



            CreateMap<OrderItemCreateDto, OrderItem>();
            CreateMap<OrderItemDto, OrderItem>().ReverseMap();

            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryCreateDto, Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();
            CreateMap<CategoryListDto, Category>().ReverseMap();


            CreateMap<PaymentCreateDto, Payment>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
        }




            ////////////////////////////////////////////

            // CategoryIds → BookCategories Çeviren Custom Resolver
            public class BookCategoryResolver : IValueResolver<BookCreateDto, Book, ICollection<BookCategory>>
            {
            public ICollection<BookCategory> Resolve(BookCreateDto source, Book destination, ICollection<BookCategory> destMember, ResolutionContext context)
            {
                return source.CategoryIds.Select(id => new BookCategory { CategoryId = id }).ToList();
            }

            }


    }
}
