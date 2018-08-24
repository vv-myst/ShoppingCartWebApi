using System.Collections.Generic;

namespace ShoppingCartWebApi.Models.Interfaces
{
    public interface IShoppingCart
    {
        IList<IItem> ItemList { get; }

        /// <summary>
        ///     Dictionary where the ItemId is the key and ItemCount is the value
        /// </summary>
        IDictionary<int, int> ItemCountMap { get; }

        /// <summary>
        ///     Total number of items in the shopping cart
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        ///     Total value of all the items in shopping cart
        /// </summary>
        decimal TotalValue { get; }
    }
}