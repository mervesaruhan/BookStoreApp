using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IBookRepository
    {
        Task<Book> AddBookAsync(Book book);
        Task<Book> UpdateBookAsync(int id,Book book);
        Task<List<Book>> GetAllBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<List<Book>> GetBooksByGenreAsync(string genre);
        Task<List<Book>> SearchBooksAsync(string searchText);
        Task<bool> DeleteBookAsync(int id);


    }
}
