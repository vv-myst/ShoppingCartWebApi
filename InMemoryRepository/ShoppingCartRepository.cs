using System.Collections.Generic;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.InMemoryRepository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public ShoppingCartRepository()
        {
            InMemoryShoppingCart = new ShoppingCart(new List<Item>());
        }

        /// <inheritdoc />
        /// <summary>
        ///     An in memory storage of Shopping Cart and its changes
        /// </summary>
        public ShoppingCart InMemoryShoppingCart { get; }

        /// <inheritdoc />
        /// <summary>
        ///     Update an existing entry in the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Update(ShoppingCart entity)
        {
            InMemoryShoppingCart.ItemCount = entity.ItemCount;
            InMemoryShoppingCart.TotalValue = entity.TotalValue;
            InMemoryShoppingCart.ItemList = entity.ItemList;
            InMemoryShoppingCart.ItemCountMap = entity.ItemCountMap;
        }
    }
}