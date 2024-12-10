using AutoMapper;
using BookStoreApp.Model.DTO;
using BookStoreApp.Model.DTO.ShoppingCartDtos;
using BookStoreApp.Model.Interface;
using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Service
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public ShoppingCartService (IShoppingCartRepository shoppingCartRepository, IMapper mapper, IBookRepository bookRepository)
        {
            _shoppingCartRepository=shoppingCartRepository;
            _mapper=mapper;
            _bookRepository=bookRepository;
        }


        public ResponseDto<ShoppingCartDto> AddShoppingCart(ShoppingCartCreateDto createCartDto)
        {
            var cart = _shoppingCartRepository.GetShoppingCartByUserId(createCartDto.UserId);//kullanıcı sepetini bulma
            foreach (var itemDto in createCartDto.Items)//kullanıcının eklemek istedigi ürüner üzerinde döngü oluşturulur
            {
                var book = _bookRepository.GetBookById(itemDto.BookId);//book idye göre kitap bilgileri kontrol edilir
                if (book == null)
                {
                    return ResponseDto<ShoppingCartDto>.Fail($"Kitap bulunamadı: {itemDto.BookId}");
                }

                var existingItem = cart.Items.FirstOrDefault(item => item.Id == itemDto.BookId);//Aynı BookId'ye sahip bir ürünün zaten sepet içinde olup olmadığını kontrol eder. varsa miktarını artırır yoksa yeni ekler
                if (existingItem != null)
                {
                    existingItem.Quantity += itemDto.Quantity;
                }
                else
                {
                    cart.Items.Add(new ShoppingCartItem
                    {
                        BookId = book.Id,
                        BookTitle = book.Title,
                        Quantity = itemDto.Quantity,
                        Price = book.Price
                    });
                }
            }
            _shoppingCartRepository.UpdateShoppingCart(cart);
            var result = _mapper.Map<ShoppingCartDto>(cart);
            return ResponseDto<ShoppingCartDto>.Succes(result); ;
        }


        public ResponseDto<ShoppingCartDto> GetCartByUserId(int userId)
        {
            var cart = _shoppingCartRepository.GetShoppingCartByUserId(userId);
            if (cart == null) return ResponseDto<ShoppingCartDto>.Fail("Sepet bulunamadı");
            var result = _mapper.Map<ShoppingCartDto>(cart);
            return ResponseDto<ShoppingCartDto>.Succes(result);
        }


        public ResponseDto<bool> ClearCart(int userId)
        {
            var isDeleted = _shoppingCartRepository.ClearCart(userId);
            if (!isDeleted)  return ResponseDto<bool>.Fail("Sepet zaten boş");
            return  ResponseDto<bool>.Succes(true);
        }



    }
}
