using BookStoreApp.Model.DTO;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Shared;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace BookStoreApp.Model.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository (AppDbContext context)
        {
            _context = context;
        }

        

        public  async Task<Book> AddBookAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
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
            existingBook.Stock = book.Stock;

            //eski kategoriler silinmeli
            _context.BookCategories.RemoveRange(existingBook.BookCategories);

            //yeni kategoriler eklenmeli
            existingBook.BookCategories = book.BookCategories;

            _context.SaveChanges();

            return existingBook;

        }




        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Include(b => b.OrderItems)
                .ToListAsync();
        }



        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .Include(b => b.OrderItems)
                .FirstOrDefaultAsync(b => b.Id == id);
        }



        public async Task<List<Book>> GetBooksByGenreAsync(string genre)
        {
            return await _context.Books
                .AsNoTracking()
                .Where(g => g.Genre.ToLower() == genre.ToLower())
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .ToListAsync();
        }




        public async Task<List<Book>> SearchBooksAsync(string searchText)
        {
            return await _context.Books.AsNoTracking()
                .Where(book =>
                    book.Title.ToLower().Contains(searchText.ToLower()) ||
                    book.Author.ToLower().Contains(searchText.ToLower()) ||
                    book.Publisher.ToLower().Contains(searchText.ToLower()))
                .Include(b => b.BookCategories)
                .ThenInclude(bc => bc.Category)
                .ToListAsync();
        }




        public async Task<bool> DeleteBookAsync(int id)
        {
            var book = await GetBookByIdAsync(id);
            if (book == null) return false;

            //kitap siparişlerde yer alıyorsa silinmemeli
            if (book.OrderItems.Any()) throw new InvalidOperationException("Kitap siparişlerde yer aldığı için silinemez");

            //ilişkili bookCategory tablosundan silinmeli
            _context.BookCategories.RemoveRange(book.BookCategories);

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return true;
        }



        //veritabanında belirli bir koşula uyan herhangi bir kayıt olup olmadığını kontrol edecek metot, generic metot
        public async Task<bool> AnyAsync(Expression<Func<Book, bool>> predicate)
        {
            return await _context.Books.AnyAsync(predicate);
        }

        public async Task<List<Book>> GetBooksByIdsAsync(List<int> bookIds)
        {
            return await _context.Books
                .Where(b => bookIds.Contains(b.Id))
                .ToListAsync();
        }

        public async Task UpdateBooksAsync(List<Book> books)
        {
            _context.Books.UpdateRange(books); // Tüm kitapları tek seferde güncelle
            await _context.SaveChangesAsync();
        }

    }
}
