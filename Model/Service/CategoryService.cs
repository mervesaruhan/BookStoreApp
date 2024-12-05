using AutoMapper;
using BookStoreApp.Model.DTO.CategoryDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public ResponseDto<CategoryDto> AddCategory(CategoryCreateDto createDto)
        {

            var existingCategort = _categoryRepository.GetCategoryByName(createDto.Name);
            if (existingCategort != null) 
            { 
                return ResponseDto<CategoryDto>.Fail("Girilen kategori zaten mevcut!"); 
            }

            var category = _mapper.Map<Category>(createDto);
            var createdCategory =_categoryRepository.AddCategory(category);
            var result = _mapper.Map<CategoryDto>(createdCategory);
            return ResponseDto<CategoryDto>.Succes(result);
        }

        public ResponseDto<List<CategoryDto>> GetAllCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            var result = _mapper.Map<List<CategoryDto>>(categories);
            return ResponseDto<List<CategoryDto>>.Succes(result);
        }


        public ResponseDto<CategoryDto> GetCategoryById(int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null) return ResponseDto<CategoryDto>.Fail("Kategori bulunamadı");
            var result = _mapper.Map<CategoryDto>(category);
            return ResponseDto<CategoryDto>.Succes(result);
        }

        public ResponseDto<bool> DeleteCategoryById(int id)
        {
            var isDeleted =_categoryRepository.DeleteCategory(id);
            if (!isDeleted) return ResponseDto<bool>.Fail("kategori sbulunamadı");
            return ResponseDto<bool>.Succes(true);

        }
    }
}
