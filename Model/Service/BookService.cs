using AutoMapper;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.BookDtos;
using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;



namespace BookStoreApp.Model.Service
{
    public class BookService : IBookService
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


        public async Task<ResponseDto<BookDto>> AddBookAsync(BookCreateDto bookDto)
        {
            try
            {

                // girilen isbn eşsiz mi
                bool isbnExist = await _bookRepository.AnyAsync(b => b.ISBN == bookDto.ISBN);
                if (isbnExist) return ResponseDto<BookDto>.Fail("Bu ISBN numarası ile kayıtlı bir kitap mevcut");


                //geçersiz kategori idleri var mı  
                var CategoryIds =await _categoryRepository.GetExistingCategoryIdsAsync(bookDto.CategoryIds);

                var invalidCategoryIds = bookDto.CategoryIds.Except(CategoryIds).ToList();

                if (invalidCategoryIds.Any())
                {
                    return ResponseDto<BookDto>.Fail("Geçersiz kategori ID'leri: " + string.Join(",", invalidCategoryIds));
                }


                // DTO'dan Entity'ye dönüşüm
                var book = _mapper.Map<Book>(bookDto);


                // Repository'de kitap ekleme - id dolması için önce ekleme yapıyoruz
                var createdBook = await _bookRepository.AddBookAsync(book);

                createdBook.BookCategories = bookDto.CategoryIds.Select(id => new BookCategory { BookId = createdBook.Id,CategoryId = id}).ToList();


                await _bookRepository.UpdateBookAsync(createdBook.Id,createdBook); // Güncellenmiş ilişkiyi kaydet

                // Entity'den DTO'ya dönüşüm
                var result = _mapper.Map<BookDto>(createdBook);

                return ResponseDto<BookDto>.Succes(result);
            }

            catch (Exception ex)
            {
                return ResponseDto<BookDto>.Fail(ex.Message);
            }
        }





        public async Task<ResponseDto<BookDto>> UpdateBookAsync(int id, UpdateBookDto updateBookDto)
        {
            try
            {
                //kitap var mı kontrol
                var bookExist = await _bookRepository.GetBookByIdAsync(id);
                if (bookExist == null) return ResponseDto<BookDto>.Fail("Güncellenecek kitap bulunamadı");

                //ISBN Benzersizlik Kontrolü (Güncellenen kitap hariç, aynı ISBN başka kitapta var mı?)
                bool isbnExist = await _bookRepository.AnyAsync(b => b.ISBN == updateBookDto.ISBN && b.Id != id);
                if (isbnExist) return ResponseDto<BookDto>.Fail("Bu ISBN numarası ile kayıtlı başka bir kitap mevcut");

                //geçersiz kategori idleri var mı
                var existingCategoryIds = await _categoryRepository.GetExistingCategoryIdsAsync(updateBookDto.CategoryIds);
                var invalidCategoryIds = updateBookDto.CategoryIds.Except(existingCategoryIds).ToList();
                if (invalidCategoryIds.Any())
                {
                    return ResponseDto<BookDto>.Fail("Geçersiz kategori ID'leri: " + string.Join(",", invalidCategoryIds));
                }



                //dto entitiy dönüşüm ve id atama
                var book = _mapper.Map<Book>(updateBookDto);
                book.Id = id;


                //Güncellenmiş BookCategory ilişkileri
                book.BookCategories = updateBookDto.CategoryIds
                    .Select(catId => new BookCategory { BookId = id, CategoryId = catId })
                    .ToList();

                var updatedBook = await _bookRepository.UpdateBookAsync(id, book);

                var result = _mapper.Map<BookDto>(updatedBook);
                return ResponseDto<BookDto>.Succes(result);

            }
            catch (Exception ex)
            {
                return ResponseDto<BookDto>.Fail(ex.Message);
            }

        }





        public async Task<ResponseDto<bool>> DeleteBookAsync(int id)
        {
            try
            {
                var isDeleted = await _bookRepository.GetBookByIdAsync(id);
                if (isDeleted == null) return ResponseDto<bool>.Fail("Silinecek kitap bulunamadı");

                var deletedBook = await _bookRepository.DeleteBookAsync(id);
                if (!deletedBook) return ResponseDto<bool>.Fail("Kitap silinirken bir hata oluştu");



                return ResponseDto<bool>.Succes(true);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseDto<bool>.Fail(ex.Message);//siparişlerde kullanılma durumunda burası hata verecek
            }
            catch (Exception ex)
            {
                return ResponseDto<bool>.Fail(ex.Message);
            }
        }





        public async Task<ResponseDto<BookDto>> GetBookByIdAsync(int id)
        {
            var bookEntity = await _bookRepository.GetBookByIdAsync(id);

            if (bookEntity == null) return ResponseDto<BookDto>.Fail("Kitap bulunamadı.");

            var bookDto = _mapper.Map<BookDto>(bookEntity);
            return ResponseDto<BookDto>.Succes(bookDto);
        }





        public async Task<ResponseDto<List<BookDto>>> GetAllBooksAsync()
        {
            var listBook = await _bookRepository.GetAllBooksAsync();
            var result = _mapper.Map<List<BookDto>>(listBook);
            return ResponseDto<List<BookDto>>.Succes(result);
        }




        public async Task<ResponseDto<List<BookDto>>> GetBooksByGenreAsync(string genre)
        {
            var listBooks = await _bookRepository.GetBooksByGenreAsync(genre);
            if (listBooks == null || !listBooks.Any()) return ResponseDto<List<BookDto>>.Fail("Bu türe ait kitap bulunamadı");

            var result = _mapper.Map<List<BookDto>>(listBooks);
            return ResponseDto<List<BookDto>>.Succes(result);

        }




        public async Task<ResponseDto<List<BookDto>>> SearchBooksAsync(string searchText)
        {
            var listBooks = await _bookRepository.SearchBooksAsync(searchText);
            if (listBooks == null || !listBooks.Any())
            {
                return ResponseDto<List<BookDto>>.Fail("Girdiğiniz kelimeyi içeren  kitap bulunumadı");
            }
            var result = _mapper.Map<List<BookDto>>(listBooks);

            return ResponseDto<List<BookDto>>.Succes(result);
        }

    }
}
