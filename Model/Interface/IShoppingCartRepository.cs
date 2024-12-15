using BookStoreApp.Model.Entities;

namespace BookStoreApp.Model.Interface
{
    public interface IShoppingCartRepository
    {
        ShoppingCart GetShoppingCartByUserId(int userId);
        ShoppingCart UpdateShoppingCart(ShoppingCart cart);
        bool ClearCart(int userId);
        ShoppingCart? GetCartById(int id);
        ShoppingCart CreateCart(ShoppingCart cart);

    }
}
