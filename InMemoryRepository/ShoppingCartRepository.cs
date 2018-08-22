using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        ///     Update the shopping cart values.
        ///     Async - Await is not necessary here as it is just an in memory list being updated.
        ///     Ideally this would be a connection to a backend database service.
        ///     The async - await is written to just mimic the backend database by
        ///     introducing artificial latency in finishing the task.
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        public async Task<ShoppingCart> Update(ShoppingCart shoppingCart)
        {
            await Task.Run(() =>
            {
                InMemoryShoppingCart.ItemCount = shoppingCart.ItemCount;
                InMemoryShoppingCart.TotalValue = shoppingCart.TotalValue;
                InMemoryShoppingCart.ItemList = shoppingCart.ItemList;
                InMemoryShoppingCart.ItemCountMap = shoppingCart.ItemCountMap;
                Thread.Sleep(2000);
            });
            
            return InMemoryShoppingCart;
        }
    }
}