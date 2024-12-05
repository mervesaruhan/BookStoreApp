using BookStoreApp.Model.DTO.CategoryDtos;
using BookStoreApp.Model.DTO;

namespace BookStoreApp.Model.Interface
{
    public interface ICategoryService
    {
        ResponseDto<CategoryDto> AddCategory(CategoryCreateDto createDto);
        ResponseDto<List<CategoryDto>> GetAllCategories();
        ResponseDto<CategoryDto> GetCategoryById(int id);
        ResponseDto<bool> DeleteCategoryById(int id);
    }
}
