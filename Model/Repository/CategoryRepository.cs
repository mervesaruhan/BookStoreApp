using BookStoreApp.Model.Entities;
using System.ComponentModel.DataAnnotations;
using BookStoreApp.Model.Interface;
using Microsoft.EntityFrameworkCore;
using BookStoreApp.Model.Shared;

namespace BookStoreApp.Model.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
       


        public async Task<Category> AddCategoryAsync(Category category)
        {
            bool categoryExist = await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower());
            if (categoryExist) throw new InvalidOperationException("Girilen kategory zaten mevcut");

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }


        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                throw new KeyNotFoundException(" Girilen ID' de kategori bulunamadı.");
            }
            return category;
        }



        public async Task<Category?> GetCategoryByNameAsync (string name)
        {
            var category = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

            if (category == null) throw new KeyNotFoundException($"'{name}': isimde kategori bulunamadı.");

            return category;
           
        }




        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.AsNoTracking()
                .ToListAsync();
        }



        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                .Include(c => c.BookCategories)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) throw new KeyNotFoundException($"ID: '{id} ile kateori kaydı bulunamadı.");

            //ilişkili bookcategory kayıtlarının silinmesi lazım
            _context.BookCategories.RemoveRange(category.BookCategories);

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }



        public async Task<List<int>> GetExistingCategoryIdsAsync(List<int> categoryIds)
        {
            return await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();
        }

    }
}
