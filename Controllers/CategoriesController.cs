using BookStoreApp.Model.DTO.CategoryDtos;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController (ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }



        [HttpPost]
        public async Task<IActionResult> AddCategoryAsync([FromBody]CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _categoryService.AddCategoryAsync(categoryDto);
            if (response.Data == null) return BadRequest(response);
            //return CreatedAtAction(nameof(GetCategoryByIdAsync), new { id = response.Data.Id }, response);
            return Ok(response);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var response = await _categoryService.GetAllCategoriesAsync();
            if (response.Data == null || !response.Data.Any())  return NotFound(response); 
            return Ok(response);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByIdAsync(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }




        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCategoryByNamedAsync(string name)
        {
            var response = await _categoryService.GetCategoryByNameAsync(name);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }




        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoryByIdAsync(int id)
        {
            var response = await _categoryService.DeleteCategoryByIdAsync(id);
            if (!response.Data) return NotFound(response);
            return Ok(new
            {
                Message = "Kategori başarıyla silindi.",
                Success = true
            });

        }
    }
}
