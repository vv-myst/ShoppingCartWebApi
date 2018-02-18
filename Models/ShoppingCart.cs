using System.Collections.Generic;
using System.Linq;

namespace ShoppingCartWebApi.Models
{
    public class ShoppingCart
    {
        public ShoppingCart(IList<Item> itemList)
        {
            ItemList = itemList;
            ItemCountMap = new Dictionary<int, int>();
            UpdateItemCountMapUponAdd();
            UpdateShoppingCart();
        }

        /// <summary>
        ///     List of items in the shopping cart
        /// </summary>
        public IList<Item> ItemList { get; internal set; }

        /// <summary>
        ///     Dictionary where the ItemId is the key and ItemCount is the value
        /// </summary>
        public IDictionary<int, int> ItemCountMap { get; internal set; }

        /// <summary>
        ///     Total number of items in the shopping cart
        /// </summary>
        public int ItemCount { get; internal set; }

        /// <summary>
        ///     Total value of all the items in shopping cart
        /// </summary>
        public decimal TotalValue { get; internal set; }

        /// <summary>
        ///     Updates the total quantity of each item in the ItemCountMap dictionary
        ///     when a new item is added to the shopping cart
        /// </summary>
        public void UpdateItemCountMapUponAdd()
        {
            foreach (var item in ItemList)
                ItemCountMap[item.Id] = ItemList.Count(x => x.Id == item.Id);
        }

        /// <summary>
        ///     Updates the total quantity of each item in the ItemCountMap dictionary
        ///     when items of a particular id are deleted from the shopping cart.
        /// </summary>
        /// <param name="itemId"></param>
        public void UpdateItemCountMapUponDelete(int itemId)
        {
            if (ItemList.Count == 0)
                ClearItemCountMap();
            else
                ItemCountMap[itemId] = 0;
        }

        /// <summary>
        ///     Clears all the entries in the ItemCountMap dictionary
        /// </summary>
        public void ClearItemCountMap()
        {
            ItemCountMap.Clear();
        }

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