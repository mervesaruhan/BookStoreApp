using BookStoreApp.Model.DTO.CategoryDtos;
using BookStoreApp.Model.DTO;

namespace BookStoreApp.Model.Interface
{
    public interface ICategoryService
    {
        Task<ResponseDto<CategoryDto>> AddCategoryAsync(CategoryCreateDto createDto);
        Task<ResponseDto<List<CategoryDto>>> GetAllCategoriesAsync();
        Task<ResponseDto<CategoryDto>> GetCategoryByIdAsync(int id);
        Task<ResponseDto<bool>> DeleteCategoryByIdAsync(int id);
        Task<ResponseDto<CategoryDto>> GetCategoryByNameAsync(string name);
    }
}
