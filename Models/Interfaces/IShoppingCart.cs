namespace ShoppingCartWebApi.Models.Interfaces
{
    public interface IShoppingCart
    {
        /// <summary>
        ///     Updates the total quantity of each item in the ItemCountMap dictionary
        ///     when a new item is added to the shopping cart
        /// </summary>
        void UpdateItemCountMapUponAdd();
        
        /// <summary>
        ///     Updates the total quantity of each item in the ItemCountMap dictionary
        ///     when items of a particular id are deleted from the shopping cart.
        /// </summary>
        /// <param name="itemId"></param>
        void UpdateItemCountMapUponDelete(int itemId);
        
        /// <summary>
        ///     Clears all the entries in the ItemCountMap dictionary
        /// </summary>
        void ClearItemCountMap();
        
        /// <summary>
        ///     Updates the remaining properties of the shopping cart when an item
        ///     is either added or deleted from the cart.
        /// </summary>
        void UpdateShoppingCart();
    }
}