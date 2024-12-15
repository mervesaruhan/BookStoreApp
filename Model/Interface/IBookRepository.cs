using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IBookRepository
    {
        Book AddBook(Book book);
        Book UpdateBook(Book book);
        List<Book> GetAllBooks();
        Book? GetBookById(int id);
        List<Book> GetBooksByGenre(string genre);
        List<Book> SearchBooks(string searchText);
        bool DeleteBook(int id);


    }
}
