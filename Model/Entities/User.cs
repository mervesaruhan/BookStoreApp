namespace BookStoreApp.Model.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; }= null!;
        public UserRole Role { get; set; }


        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

    }

    public enum UserRole
    {
        Admin,
        Customer
    }
}
