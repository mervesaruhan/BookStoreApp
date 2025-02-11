namespace BookStoreApp.Model.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        //çoka - çok ilişki: bir kategori birden çok kitaba ait olabilir
        public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();


    }
}
