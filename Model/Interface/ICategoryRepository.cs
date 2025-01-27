using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category?> GetCategoryByNameAsync(string name);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<bool> DeleteCategoryAsync(int id);
    }
}
