using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public CategoryRepository() { }

        private readonly List<Category> _categories = new();

        public Category AddCategory(Category category)
        {
            if (_categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            { 
                throw new ValidationException("Bu kategori zaten mevcut"); 
            }
            category.Id = _categories.Count + 1;
            _categories.Add(category);
            return category;
        }

        public Category GetCategoryById(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                throw new ValidationException(" Girilen ID' de kategori bulunamadı.");
            }
            return category;
        }


        public Category GetCategoryByName(string name)
        {
            var category = _categories.FirstOrDefault(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (category == null) throw new ValidationException("Girilen isimde category yoktur");
            return category;
        }



        public List<Category> GetAllCategories()
        {
            return _categories;
        }


        public bool DeleteCategory(int id)
        {
            var category =GetCategoryById(id);
            if (category == null) { return false; }
            _categories.Remove(category);
            return true;
        }
    }
}
