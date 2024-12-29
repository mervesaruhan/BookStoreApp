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
           services.AddSingleton<IUserRepository, UserRepository>();

           services.AddSingleton<IBookRepository, BookRepository>();
           services.AddScoped<IBookService, BookService>();

           services.AddSingleton<IOrderRepository, OrderRepository>();
           services.AddScoped<IOrderService, OrderService>();

           services.AddSingleton<ICategoryRepository, CategoryRepository>();
           services.AddScoped<ICategoryService, CategoryService>();

           services.AddSingleton<IPaymentRepository, PaymentRepository>();
           services.AddScoped<IPaymentService, PaymentService>();
        }
    }
}
