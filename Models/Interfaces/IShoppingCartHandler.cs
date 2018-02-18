namespace ShoppingCartWebApi.Models.Interfaces
{
    public interface IShoppingCartHandler
    {
        /// <summary>
        ///     Adds an item to the shopping cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="item"></param>
        void AddItemToShoppingCart(ShoppingCart shoppingCart, Item item);

        /// <summary>
        ///     Delete all items from the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="itemId"></param>
        void DeleteItemsFromShoppingCart(ShoppingCart shoppingCart, int itemId);
        
        /// <summary>
        ///     Update the quantity of items in the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="itemId"></param>
        /// <param name="itemCount"></param>
        void UpdateItemQuantityInShoppingCart(ShoppingCart shoppingCart, int itemId, int itemCount);

        /// <summary>
        ///     Remove all items from the shopping cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        void EmptyShoppingCart(ShoppingCart shoppingCart);
        
        /// <summary>
        ///     Check if an item with the specific itemId exists in the shopping cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        bool DoesItemExist(ShoppingCart shoppingCart, int itemId);
    }
}