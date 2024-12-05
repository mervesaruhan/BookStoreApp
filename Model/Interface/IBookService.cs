using BookStoreApp.Model.DTO.BookDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IBookService
    {
        ResponseDto<BookDto> AddBook(BookCreateDto bookDto);
        ResponseDto<BookDto> UpdateBook(int id , UpdateBookDto updateBookDto);
        ResponseDto<bool> DeleteBook(int id);
        ResponseDto<BookDto> GetBookById(int id);
        ResponseDto<List<BookDto>> GetAllBooks();
        ResponseDto<List<BookDto>> GetBooksByGenre(string genre);
        ResponseDto<List<BookDto>> SearchBooks(string searchText);

    }
}
