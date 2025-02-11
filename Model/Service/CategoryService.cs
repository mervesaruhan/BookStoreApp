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

        public async Task<ResponseDto<CategoryDto>> AddCategoryAsync(CategoryCreateDto createDto)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetCategoryByNameAsync(createDto.Name);

                return ResponseDto<CategoryDto>.Fail("Girilen kategori zaten mevcut!");

            }
            catch (KeyNotFoundException )
            {

                var category = _mapper.Map<Category>(createDto);
                var createdCategory = await _categoryRepository.AddCategoryAsync(category);
                var result = _mapper.Map<CategoryDto>(createdCategory);

                return ResponseDto<CategoryDto>.Succes(result);
            }

        }



        public async Task<ResponseDto<List<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            var result = _mapper.Map<List<CategoryDto>>(categories);

            return ResponseDto<List<CategoryDto>>.Succes(result);
        }



        public async Task<ResponseDto<CategoryDto>> GetCategoryByIdAsync(int id)
        {
            try
            {
                var category = await  _categoryRepository.GetCategoryByIdAsync(id);

                var result = _mapper.Map<CategoryDto>(category);

                return ResponseDto<CategoryDto>.Succes(result);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto<CategoryDto>.Fail(ex.Message);
            }
            
        }



        public async Task<ResponseDto<bool>> DeleteCategoryByIdAsync(int id)
        {
            try
            {
                var result = await _categoryRepository.DeleteCategoryAsync(id);

                return ResponseDto<bool>.Succes(true);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto<bool>.Fail(ex.Message);
            }

        }


        public async Task<ResponseDto<CategoryDto>> GetCategoryByNameAsync(string name)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByNameAsync(name);
                var result = _mapper.Map<CategoryDto>(category);
                return ResponseDto<CategoryDto>.Succes(result);
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseDto<CategoryDto>.Fail(ex.Message);
            }
        }




    }
}
