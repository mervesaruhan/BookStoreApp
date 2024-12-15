using BookStoreApp.Model.DTO.ShoppingCartDtos;
using BookStoreApp.Model.DTO;

namespace BookStoreApp.Model.Interface
{
    public interface IShoppingCartService
    {
        ResponseDto<ShoppingCartDto> AddShoppingCart(ShoppingCartCreateDto createCartDto);
        ResponseDto<ShoppingCartDto> GetCartByUserId(int userId);
        ResponseDto<bool> ClearCart(int userId);
        ResponseDto<ShoppingCartDto> GetCartById(int id);
    }
}
