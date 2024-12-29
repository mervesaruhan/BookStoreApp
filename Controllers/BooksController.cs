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



        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var response = _bookService.GetAllBooks();
            if (response.Data == null || !response.Data.Any()) return NotFound(response);

            return Ok(response);
        }



        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var response = _bookService.GetBookById(id);
            if (response.Data == null) return NotFound(response);

            return Ok(response);
        }


        [HttpGet ("genre/{genre}")]
        public IActionResult GetBooksByGenre(string genre)
        {
            var response = _bookService.GetBooksByGenre(genre);
            if (response.Data == null || !response.Data.Any()) return NotFound(Response);
            return Ok(response);
        }


        [HttpGet("search / {searchText}")]
        public IActionResult SearchBook(string searchText)
        {
            var response = _bookService.SearchBooks(searchText);
            if (response.Data == null || !response.Data.Any()) { return NotFound(Response); }
            return Ok(response);
        }




        [HttpPost]
        public IActionResult AddBook(BookCreateDto bookDto)
        {
            if (!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi");

            var response = _bookService.AddBook(bookDto);
            if (response.Data == null ) return BadRequest(response);

            return CreatedAtAction(nameof(GetBookById), new { id = response.Data.Id }, response);
        }


        [HttpPut ("{id}")]
        public IActionResult UpdateBook(int id,  [FromBody] UpdateBookDto updateBookDto)
        {
            if(!ModelState.IsValid) return BadRequest("Geçersiz veri gönderildi");

            var response = _bookService.UpdateBook(id, updateBookDto);
            if (response.Data == null) return NotFound("Kitap bulunamadı!");
            return Ok(response);

        }


        [HttpDelete ("{id}")]
        public IActionResult DeleteBook(int id)
        {
            var response = _bookService?.DeleteBook(id);
            if (!response.Data ) return BadRequest(response);
            return NoContent();
        }
    }
}
