using BookStoreApp.Model.DTO.BookDtos;
using BookStoreApp.Model.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController (IBookService bookService)
        {
            _bookService = bookService;
        }



        [HttpPost]
        public async Task<IActionResult> AddBookAsync(BookCreateDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi");

            var response = await _bookService.AddBookAsync(bookDto);
            if (response.Data == null) return BadRequest(response);

            //return CreatedAtAction(nameof(GetBookByIdAsync), new { id = response.Data.Id }, response);
            return Ok(response);
            
        }



        [HttpGet]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var response = await _bookService.GetAllBooksAsync();
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            return Ok(response);
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookByIdAsync(int id)
        {
            var response = await _bookService.GetBookByIdAsync(id);
            if (response.Data == null) return NotFound(response);

            return Ok(response);
        }


        [HttpGet ("genre/{genre}")]
        public async Task<IActionResult> GetBooksByGenreAsync(string genre)
        {
            var response = await _bookService.GetBooksByGenreAsync(genre);
            if (response.Data == null || !response.Data.Any()) return NotFound(response);
            return Ok(response);
        }


        [HttpGet("search/{searchText}")]
        public async Task<IActionResult> SearchBookAsync(string searchText)
        {
            var response = await _bookService.SearchBooksAsync(searchText);
            if (response.Data == null || !response.Data.Any()) { return NotFound(response); }
            return Ok(response);
        }





        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateBookAsync(int id,  [FromBody] UpdateBookDto updateBookDto)
        {
            if(!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi");

            var response = await _bookService.UpdateBookAsync(id, updateBookDto);
            if (response.Data == null) return NotFound("Kitap bulunamadı!");
            return Ok(response);

        }


        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteBookAsync(int id)
        {
            var response =await  _bookService.DeleteBookAsync(id);
            if (!response.Data ) return NotFound(response);
            return NoContent();
        }
    }
}
