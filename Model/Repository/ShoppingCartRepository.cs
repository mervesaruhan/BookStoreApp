using BookStoreApp.Model.Entities;
using BookStoreApp.Model.Interface;

namespace BookStoreApp.Model.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly List<ShoppingCart> _shoppingCarts = new();


        public ShoppingCart GetShoppingCartByUserId (int userId)
        {
            return _shoppingCarts.FirstOrDefault(c => c.UserId == userId) ?? new ShoppingCart { UserId = userId };

        }


        public ShoppingCart UpdateShoppingCart(ShoppingCart cart)
        {
            var existingCart = GetShoppingCartByUserId(cart.UserId);
            if (existingCart == null)
            {
                _shoppingCarts.Add(cart);
                return cart;
            }
            existingCart.Items = cart.Items;
            return existingCart;
        }


        public bool ClearCart(int userId)
        {
            var cart = GetShoppingCartByUserId(userId);
            if (cart == null) { return false; }
            _shoppingCarts.Remove(cart);
            return true;
        }
            
    }
}
