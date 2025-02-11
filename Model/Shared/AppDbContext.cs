using BookStoreApp.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookStoreApp.Model.Shared
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookCategory> BookCategories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookCategory>().HasKey(bc => new { bc.BookId, bc.CategoryId }); // Composite Key
            modelBuilder.Entity<BookCategory>().HasOne(bc => bc.Book).WithMany(b => b.BookCategories).HasForeignKey(bc => bc.BookId);
            modelBuilder.Entity<BookCategory>().HasOne(bc => bc.Category).WithMany(c => c.BookCategories).HasForeignKey(bc => bc.CategoryId);
            

            modelBuilder.Entity<Order>().HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);

            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Order).WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);// sipariş silinirse orderıtem'ler de silinsin
            modelBuilder.Entity<OrderItem>().HasOne(oi => oi.Book).WithMany(b => b.OrderItems)
                .HasForeignKey(Oi => Oi.BookId).OnDelete(DeleteBehavior.Restrict); // book silinirse ıtems silinmesin

            modelBuilder.Entity<Payment>().HasOne(p => p.User).WithMany(u => u.Payments).HasForeignKey( p => p.UserId);
            modelBuilder.Entity<Payment>().HasOne(p => p.Order).WithOne(O => O.Payment)
                .HasForeignKey<Payment>(p => p.OrderId).OnDelete(DeleteBehavior.Cascade); // sipariş silinirse ödeme de silinir
            modelBuilder.Entity<Payment>().Property(p => p.PaymentMethod).HasConversion<string>();
            modelBuilder.Entity<Payment>().Property(p => p.PaymentStatus).HasConversion<string>();


            modelBuilder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Category>().Property(c => c.Description).HasMaxLength(500);





            modelBuilder.Entity<Book>().Property(b => b.Price).HasPrecision(18,2);
            modelBuilder.Entity<Book>().Property(b => b.Stock).HasDefaultValue(0);
            modelBuilder.Entity<Book>().Property(b => b.Title).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.Author).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.Publisher).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.Genre).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Book>().Property(b => b.ISBN).HasMaxLength(50);





            modelBuilder.Entity<OrderItem>().Property(oi => oi.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<OrderItem>().Property(oi => oi.Quantity).IsRequired().HasDefaultValue(1);


            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<Payment>().Property(p => p.PaymentDate).HasDefaultValueSql("getdate()");


            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();
            modelBuilder.Entity<User>().Property(u => u.Role).HasDefaultValue(UserRole.Customer);
            modelBuilder.Entity<User>().Property(u => u.Password).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.PasswordHash).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.PasswordSalt).IsRequired();


            modelBuilder.Entity<Order>().Property(o =>o.TotalPrice).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.OrderDate).HasDefaultValueSql("getdate()");
            modelBuilder.Entity<Order>().Property(o => o.Status).HasConversion<string>();

            base.OnModelCreating(modelBuilder);


        }

    }
}
