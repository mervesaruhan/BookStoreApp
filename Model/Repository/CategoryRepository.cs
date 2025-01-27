using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private static readonly List<Category> _categories = new();

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
            new Category { Id = 6, Name = "Aşk" }
        });
        }


        public async Task<Category> AddCategoryAsync(Category category)
        {
            if (_categories.Any(c => c.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase)))
            { 
                throw new InvalidOperationException("Bu kategori zaten mevcut"); 
            }
            category.Id = _categories.Count + 1;
            _categories.Add(category);
            return await Task.FromResult(category);
        }


        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                throw new KeyNotFoundException(" Girilen ID' de kategori bulunamadı.");
            }
            return await Task.FromResult(category);
        }



        public async Task<Category?> GetCategoryByNameAsync (string name)
        {
            var categories = _categories.FirstOrDefault(n => n.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return await Task.FromResult(categories);
           
        }




        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await Task.FromResult( _categories);
        }



        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await GetCategoryByIdAsync(id);
            if (category == null) { return false; }
            _categories.Remove(category);

            return await Task.FromResult(true);
        }
    }
}
