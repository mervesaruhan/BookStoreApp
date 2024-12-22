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
        public IActionResult AddCategory([FromBody]CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = _categoryService.AddCategory(categoryDto);
            if (response.Data == null) return BadRequest(response);
            return CreatedAtAction(nameof(GetCategoryById), new { id = response.Data.Id }, response); ;
        }



        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var response = _categoryService.GetAllCategories();
            if (response.Data == null || !response.Data.Any())  return NotFound(response); 
            return Ok(response);
        }



        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var response = _categoryService.GetCategoryById(id);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }




        [HttpGet("name/{name}")]
        public IActionResult GetCategoryByNamed(string name)
        {
            var response = _categoryService.GetCategoryByName(name);
            if (response.Data == null) return NotFound(response);

            return Ok(response);

        }




        [HttpDelete("{id}")]
        public IActionResult DeleteCategoryById(int id)
        {
            var response = _categoryService.DeleteCategoryById(id);
            if (!response.Data) return NotFound(response);
            return NoContent();

        }
    }
}
