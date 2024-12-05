using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface ICategoryRepository
    {
        Category AddCategory(Category category);
        Category GetCategoryById(int id);
        Category GetCategoryByName(string name);
        List<Category> GetAllCategories();
        bool DeleteCategory(int id);
    }
}
