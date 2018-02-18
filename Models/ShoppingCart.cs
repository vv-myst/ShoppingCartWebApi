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

        private void UpdateItemCountMapUponAdd()
        {
            foreach (var item in ItemList)
                ItemCountMap[item.Id] = ItemList.Count(x => x.Id == item.Id);
        }

        private void UpdateShoppingCart()
        {
            ItemCount = ItemList.Count;
            TotalValue = ItemList.Sum(item => item.Value);
        }
    }
}