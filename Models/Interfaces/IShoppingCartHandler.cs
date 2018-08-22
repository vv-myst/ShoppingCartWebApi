using System.Threading.Tasks;

namespace ShoppingCartWebApi.Models.Interfaces
{
    public interface IShoppingCartHandler
    {
        /// <summary>
        ///     Adds an item to the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="item">Item to be added</param>
        Task<ShoppingCart> AddItemToShoppingCart(ShoppingCart shoppingCart, Item item);

        /// <summary>
        ///     Delete all items from the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be deleted</param>
        Task<ShoppingCart> DeleteItemsFromShoppingCart(ShoppingCart shoppingCart, int itemId);
        
        /// <summary>
        ///     Update the quantity of items in the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be updated</param>
        /// <param name="itemCount">Quantity of the item to be updated</param>
        Task<ShoppingCart> UpdateItemQuantityInShoppingCart(ShoppingCart shoppingCart, int itemId, int itemCount);

        /// <summary>
        ///     Remove all items from the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        void EmptyShoppingCart(ShoppingCart shoppingCart);
        
        /// <summary>
        ///     Check if an item with the specific itemId exists in the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be matched</param>
        /// <returns></returns>
        bool DoesItemExist(ShoppingCart shoppingCart, int itemId);
    }
}