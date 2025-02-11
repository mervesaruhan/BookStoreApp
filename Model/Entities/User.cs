namespace BookStoreApp.Model.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; }= null!;
        public UserRole Role { get; set; }

        //bire - çok ilişki : bir kullanıcı birden çok siparişe sahip olabilir
        public ICollection<Order> Orders { get; set; } = new List<Order>();

        //bire çok ilişki: bir kullanıcının birden çok ödemesi olabilir
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();


        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }

    public enum UserRole
    {
        Admin,
        Customer
    }
}
