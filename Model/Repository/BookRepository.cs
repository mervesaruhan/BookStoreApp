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

        public  async Task<Book> AddBookAsync(Book book)
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
           await Task.Run(() => _books.Add(book));

            // 5. Başarılı Yanıt
            return book;
        }

        public async Task<Book> UpdateBookAsync(int id, Book book)
        {
            var existingBook = await GetBookByIdAsync(id);
            if (existingBook == null)
            {
                throw new Exception($"Kitap bulunamadı: ID {book.Id}");
            }

            existingBook.Author = book.Author;
            existingBook.Title = book.Title;
            existingBook.Price = book.Price;
            existingBook.Genre = book.Genre;
            existingBook.ISBN = book.ISBN;
            existingBook.CategoryNames = book.CategoryNames;
            existingBook.Stock = book.Stock;

            return await Task.FromResult(existingBook);

        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            await Task.CompletedTask;
            return  _books;
        }



        public async Task<Book?> GetBookByIdAsync(int id)
        {
            var books = _books.FirstOrDefault(x => x.Id == id);
            return await Task.FromResult(books);
        }


        public async Task<List<Book>> GetBooksByGenreAsync(string genre)
        {
            var books = _books.Where(g => g.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase)).ToList();

            return await Task.FromResult(books);
        }

        public async Task<List<Book>> SearchBooksAsync(string searchText)
        {
            return await Task.Run(() =>_books
                .Where(book =>
                    book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    book.Publisher.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                .ToList());
        }



        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await GetBookByIdAsync(id);
            if (book == null) return false;
            _books.Remove(book);

            await Task.CompletedTask;
            return true;
        }


    }
}
