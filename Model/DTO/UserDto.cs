using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.DTO
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public UserRole Role { get; set; }

    }
}
