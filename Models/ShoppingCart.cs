#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi ShoppingCart.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-16 8:33 
// 2018-02-15 11:06 

#endregion

using System.Collections.Generic;
using System.Linq;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.Models
{
    public class ShoppingCart : IShoppingCart
    {
        public ShoppingCart(IList<Item> itemList)
        {
            ItemList = itemList;
            ItemCountMap = new Dictionary<int, int>();
            UpdateItemCountMapUponAdd();
            UpdateShoppingCart();
        }

        public IList<Item> ItemList { get; }
        public IDictionary<int, int> ItemCountMap { get; }
        public int ItemCount { get; private set; }
        public decimal TotalValue { get; private set; }

        /// <inheritdoc />
        /// <summary>
        ///     Updates the total quantity of each item in the ItemCountMap dictionary
        ///     when a new item is added to the shopping cart
        /// </summary>
        public void UpdateItemCountMapUponAdd()
        {
            foreach (var item in ItemList) 
                ItemCountMap[item.Id] = ItemList.Count(x => x.Id == item.Id);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Updates the total quantity of each item in the ItemCountMap dictionary
        ///     when items of a particular id are deleted from the shopping cart.
        /// </summary>
        /// <param name="itemId"></param>
        public void UpdateItemCountMapUponDelete(int itemId)
        {
            if(ItemList.Count == 0)
                ClearItemCountMap();
            else
                ItemCountMap[itemId] = 0;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Clears all the entries in the ItemCountMap dictionary
        /// </summary>
        public void ClearItemCountMap()
        {
            ItemCountMap.Clear();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Updates the remaining properties of the shopping cart when an item
        ///     is either added or deleted from the cart.
        /// </summary>
        public void UpdateShoppingCart()
        {
            ItemCount = ItemList.Count;
            TotalValue = ItemList.Sum(item => item.Value);
        }
    }
}