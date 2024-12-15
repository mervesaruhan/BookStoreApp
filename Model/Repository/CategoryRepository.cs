using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly List<Category> _categories = new();

        public CategoryRepository()
        {
            // Hazır kategoriler ekleniyor
            _categories.AddRange(new List<Category>
        {
            new Category { Id = 1, Name = "Roman" },
            new Category { Id = 2, Name = "Bilim Kurgu" },
            new Category { Id = 3, Name = "Tarih" },
            new Category { Id = 4, Name = "Kişisel Gelişim" },
            new Category { Id = 5, Name = "Çocuk" },
            new Category { Id = 5, Name = "Aşk" }
        });
        }


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


        public Category? GetCategoryByName(string name)
        {
            return _categories.FirstOrDefault(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
           
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
