using System.Collections.Generic;

namespace ShoppingCartWebApi.Models.Extensions
{
    public static class ShoppingCartExtensions
    {
        public static Dictionary<int, int> UpdateItemCountMap(this Dictionary<int, int> itemCountMap,
            IEnumerable<Item> ItemList)
        {
            foreach (var item in ItemList)
                if (itemCountMap.ContainsKey(item.Id))
                    itemCountMap[item.Id]++;
                else
                    itemCountMap.Add(item.Id, 1);

            return itemCountMap;
        }
    }
}