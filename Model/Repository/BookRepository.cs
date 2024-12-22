using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using System.ComponentModel.DataAnnotations;

namespace BookStoreApp.Model.Repository
{
    public class BookRepository : IBookRepository
    {

        public BookRepository() { }

        private readonly List<Book> _books = new();

        public  Book AddBook(Book book)
        {
            // 1. Doğrulama
            if (string.IsNullOrEmpty(book.Title) || string.IsNullOrEmpty(book.Author))
            {
                throw new ValidationException("Kitap adı ve yazar bilgisi boş olamaz");
            }

            // 2. Benzersizlik Kontrolü (ISBN)
            if (_books.Any(b => b.ISBN == book.ISBN))
            {
                throw new ValidationException("Bu ISBN numarasına sahip bir kitap zaten mevcut.");
            }

            // 3. Kitaba ID Atama
            book.Id = _books.Count + 1;


            // 4. Kitap Ekleme
            _books.Add(book);

            // 5. Başarılı Yanıt
            return book;
        }

        public Book UpdateBook(Book book)
        {
            var existingBook = GetBookById(book.Id);
            if (existingBook == null) throw new Exception("Kitap bulunamadı");

            existingBook.Author = book.Author;
            existingBook.Title = book.Title;
            existingBook.Price = book.Price;
            existingBook.Genre = book.Genre;
            existingBook.ISBN = book.ISBN;
            existingBook.Categories = book.Categories;
            existingBook.Stock = book.Stock;

            return existingBook;

        }

        public List<Book> GetAllBooks()
        {
            return _books;
        }



        public Book? GetBookById(int id)
        {
            return _books.FirstOrDefault(x => x.Id == id);
        }

        public List<Book> GetBooksByGenre(string genre)
        {
            var books = _books.Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

            return books;
        }

        public List<Book> SearchBooks(string searchText)
        {
            return _books
                .Where(book =>
                    book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    book.Publisher.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }



        public bool DeleteBook(int id)
        {
            var book = GetBookById(id);
            if (book == null) return false;
            _books.Remove(book);
            return true;
        }


    }
}
