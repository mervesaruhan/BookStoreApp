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

            var existingCategory = await _categoryRepository.GetCategoryByNameAsync(createDto.Name);
            if (existingCategory != null) 
            { 
                return ResponseDto<CategoryDto>.Fail("Girilen kategori zaten mevcut!"); 
            }

            var category = _mapper.Map<Category>(createDto);
            var createdCategory = await _categoryRepository.AddCategoryAsync(category);
            var result = _mapper.Map<CategoryDto>(createdCategory);

            return ResponseDto<CategoryDto>.Succes(result);
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
                if (category == null) return ResponseDto<CategoryDto>.Fail("Kategori bulunamadı");

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
            var isDeleted = await _categoryRepository.DeleteCategoryAsync(id);
            if (!isDeleted) return ResponseDto<bool>.Fail("kategori bulunamadı");
            return ResponseDto<bool>.Succes(true);

        }


        public async Task<ResponseDto<CategoryDto>> GetCategoryByNameAsync(string name)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(name);
            if (category == null) return ResponseDto<CategoryDto>.Fail("Girilen isimde kategori bulunamamıştır.");

            var result = _mapper.Map<CategoryDto>(category);
            return ResponseDto<CategoryDto>.Succes(result);
        }

    }
}
