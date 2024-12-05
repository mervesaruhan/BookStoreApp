namespace BookStoreApp.Model.DTO.CategoryDtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string name { get; set; } = null!;
        public string? description { get; set; }
    }
}
