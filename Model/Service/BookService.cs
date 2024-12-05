using AutoMapper;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.BookDtos;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;
using System.ComponentModel.DataAnnotations;



namespace BookStoreApp.Model.Service
{
    public class BookService:IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryRepository _categoryRepository;

        public BookService(IBookRepository bookRepository, IMapper mapper, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }


        public ResponseDto<BookDto> AddBook(BookCreateDto bookDto)
        {
            //kategori validasyonu
            var categories = bookDto.CategoryNames
                .Select(name => _categoryRepository.GetCategoryByName(name))
                .Where(c =>  c != null).ToList();

            if (categories.Count != bookDto.CategoryNames.Count)
            {
                return ResponseDto<BookDto>.Fail("Geçersiz kategori isimleri bulundu.");
            }

            // DTO'dan Entity'ye dönüşüm
            var book = _mapper.Map<Book>(bookDto);
            book.Categories = categories;

            // Repository'de kitap ekleme
            var createdBook = _bookRepository.AddBook(book);

            // Entity'den DTO'ya dönüşüm
            var result = _mapper.Map<BookDto>(createdBook);

            return ResponseDto<BookDto>.Succes(result);
        }



        public ResponseDto<BookDto> UpdateBook(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                var categories = updateBookDto.CategoryNames
                    .Select(name => _categoryRepository.GetCategoryByName(name))
                    .Where(c => c != null).ToList();

                if (categories.Count() != updateBookDto.CategoryNames.Count)
                {
                    return ResponseDto<BookDto>.Fail("Geçersiz kategori ismi girildi");
                }


                var book = _mapper.Map<Book>(updateBookDto);
                book.Id = id;
                book.Categories = categories;

                var updatedBook = _bookRepository.UpdateBook(book);
                var result = _mapper.Map<BookDto>(updatedBook);
                return ResponseDto<BookDto>.Succes(result);

            }
            catch (Exception ex)
            {
                return ResponseDto<BookDto>.Fail(ex.Message);
            }
            
        }



        public ResponseDto<bool> DeleteBook(int id)
        {
            var isDeleted = _bookRepository.DeleteBook(id);
            if (!isDeleted) return ResponseDto<bool>.Fail("Kitap bulunamadı");

            return ResponseDto<bool>.Succes(true);
        }



        public ResponseDto<BookDto> GetBookById(int id)
        {
            var bookEntity = _bookRepository.GetBookById(id);

            if (bookEntity == null) return ResponseDto<BookDto>.Fail("Kitap bulunamadı.");

            var bookDto = _mapper.Map<BookDto>(bookEntity);
            return ResponseDto<BookDto>.Succes(bookDto);
        }



        public ResponseDto<List<BookDto>> GetAllBooks()
        {
            var listBook = _bookRepository.GetAllBooks();
            var result = _mapper.Map<List<BookDto>>(listBook);
            return ResponseDto<List<BookDto>>.Succes(result);
        }



        public ResponseDto<List<BookDto>> GetBooksByGenre(string genre)
        {
            var listBooks = _bookRepository.GetBooksByGenre(genre);
            if (listBooks == null || !listBooks.Any()) return ResponseDto<List<BookDto>>.Fail("Bu türe ait kitap bulunamadı");

            var result = _mapper.Map<List<BookDto>>(listBooks);
            return ResponseDto<List<BookDto>>.Succes(result);

        }



        public ResponseDto<List<BookDto>> SearchBooks (string searchText)
        {
            var listBooks = _bookRepository.SearchBooks(searchText);
            if (listBooks == null || !listBooks.Any())
            {
                return ResponseDto<List<BookDto>>.Fail("Aradığınız kitap bulunumadı");
            }
            var result = _mapper.Map<List<BookDto>>(listBooks);

            return ResponseDto<List<BookDto>>.Succes(result);
        }

    }
}
