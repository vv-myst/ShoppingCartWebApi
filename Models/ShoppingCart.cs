using System.Collections.Generic;
using System.Linq;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.Models
{
    public class ShoppingCart : IShoppingCart
    {
        private IDictionary<int, int> itemCountMap;
        private IList<IItem> itemList;

        /// <summary>
        ///     List of items in the shopping cart
        /// </summary>
        public IList<IItem> ItemList
        {
            get
            {
                if (itemList != null)
                    return itemList;

                itemList = new List<IItem>();
                return itemList;
            }

            set
            {
                itemList = value;
                UpdateItemCountMapUponAdd();
                UpdateShoppingCart();
            }
        }

        /// <inheritdoc />
        /// <summary>
        ///     Dictionary where the ItemId is the key and ItemCount is the value
        /// </summary>
        public IDictionary<int, int> ItemCountMap
        {
            get
            {
                if (itemCountMap != null)
                    return itemCountMap;

                itemCountMap = new Dictionary<int, int>();
                return itemCountMap;
            }

            set => itemCountMap = value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Total number of items in the shopping cart
        /// </summary>
        public int ItemCount { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Total value of all the items in shopping cart
        /// </summary>
        public decimal TotalValue { get; set; }

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