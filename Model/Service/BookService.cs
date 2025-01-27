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


        public async Task<ResponseDto<BookDto>> AddBookAsync(BookCreateDto bookDto)
        {
            try
            {
                #region LINQ Eskı Yontem
                ////kategori validasyonu
                //var categories = bookDto.CategoryNames
                //    .Select(category => category.Name)
                //    .Select(name => _categoryRepository.GetCategoryByNameAsync(name))
                //    .Where(c => c != null).ToList();

                //if (!categories.Any())
                //{
                //    return ResponseDto<BookDto>.Fail("Hiçbir kategori bulunamadı.");
                //}
                #endregion
                var categoriesTask = new List<Category?>();

                foreach(var categoryName in bookDto.CategoryNames.Select(c=> c.Name))
                {
                    var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
                    if (category != null) categoriesTask.Add(category);

                }
                if (!categoriesTask.Any())
                {
                    return ResponseDto<BookDto>.Fail("Eklenen kitaba ait bir kategori blunmamaktadır.");
                }



                // DTO'dan Entity'ye dönüşüm
                var book = _mapper.Map<Book>(bookDto);
                book.CategoryNames = categoriesTask!;

                // Repository'de kitap ekleme
                var createdBook =await _bookRepository.AddBookAsync(book);

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
                #region LINQ Eskı Yontem
                //var categories = updateBookDto.CategoryNames
                //    .Select(category => category.Name)
                //    .Select(name => _categoryRepository.GetCategoryByNameAsync(name))
                //    .Where(c => c != null).ToList();

                //if (!categories.Any())
                //{
                //    return ResponseDto<BookDto>.Fail("Geçersiz kategori ismi girildi");
                //}
                #endregion
                var categoriesTask = new List<Category?>();

                foreach (var categoryName in updateBookDto.CategoryNames.Select(c => c.Name))
                {
                    var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
                    if (category != null) categoriesTask.Add(category);
                }
                if (!categoriesTask.Any())
                {
                    return ResponseDto<BookDto>.Fail("Güncellenen kitaba ait bir kategori bulunmamaktadır.");
                }


                var book = _mapper.Map<Book>(updateBookDto);
                book.Id = id;
                book.CategoryNames = categoriesTask!;

                var updatedBook = await _bookRepository.UpdateBookAsync(id,book);
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
            var isDeleted = await _bookRepository.DeleteBookAsync(id);
            if (!isDeleted) return ResponseDto<bool>.Fail("Kitap bulunamadı");

            return ResponseDto<bool>.Succes(true);
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



        public async Task<ResponseDto<List<BookDto>>> SearchBooksAsync (string searchText)
        {
            var listBooks =await _bookRepository.SearchBooksAsync(searchText);
            if (listBooks == null || !listBooks.Any())
            {
                return ResponseDto<List<BookDto>>.Fail("Aradığınız kitap bulunumadı");
            }
            var result = _mapper.Map<List<BookDto>>(listBooks);

            return ResponseDto<List<BookDto>>.Succes(result);
        }

    }
}
