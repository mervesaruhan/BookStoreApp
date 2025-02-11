using BookStoreApp.Model.DTO.BookDtos;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IBookService
    {
        Task<ResponseDto<BookDto>> AddBookAsync(BookCreateDto bookDto);
        Task<ResponseDto<BookDto>> UpdateBookAsync(int id , UpdateBookDto updateBookDto);
        Task<ResponseDto<bool>> DeleteBookAsync(int id);
        Task<ResponseDto<BookDto>> GetBookByIdAsync(int id);
        Task<ResponseDto<List<BookDto>>> GetAllBooksAsync();
        Task<ResponseDto<List<BookDto>>> GetBooksByGenreAsync(string genre);
        Task<ResponseDto<List<BookDto>>> SearchBooksAsync(string searchText);

    }
}
