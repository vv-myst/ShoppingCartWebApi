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
        ///     Update the shopping cart values
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        public void Update(ShoppingCart shoppingCart)
        {
            InMemoryShoppingCart.ItemCount = shoppingCart.ItemCount;
            InMemoryShoppingCart.TotalValue = shoppingCart.TotalValue;
            InMemoryShoppingCart.ItemList = shoppingCart.ItemList;
            InMemoryShoppingCart.ItemCountMap = shoppingCart.ItemCountMap;
        }
    }
}