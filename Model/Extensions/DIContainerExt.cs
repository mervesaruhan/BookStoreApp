using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Repository;
using BookStoreApp.Model.Service;

namespace BookStoreApp.Model.Extensions
{
    public static class DIContainerExt
    { 
        public static void AddDIContainer (this IServiceCollection services)
        {
           services.AddScoped<IUserService, UserService>();
           services.AddScoped<IUserRepository, UserRepository>();

           services.AddScoped<IBookRepository, BookRepository>();
           services.AddScoped<IBookService, BookService>();

           services.AddScoped<IOrderRepository, OrderRepository>();
           services.AddScoped<IOrderService, OrderService>();

           services.AddScoped<ICategoryRepository, CategoryRepository>();
           services.AddScoped<ICategoryService, CategoryService>();

           services.AddScoped<IPaymentRepository, PaymentRepository>();
           services.AddScoped<IPaymentService, PaymentService>();
        }
    }
}
