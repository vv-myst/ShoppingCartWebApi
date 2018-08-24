using System.Threading;
using System.Threading.Tasks;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.InMemoryRepository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private ShoppingCart shoppingCart;

        /// <inheritdoc />
        /// <summary>
        ///     An in memory storage of Shopping Cart and its changes
        /// </summary>
        public ShoppingCart InMemoryShoppingCart
        {
            get
            {
                if (shoppingCart != null)
                    return shoppingCart;

                shoppingCart = new ShoppingCart();
                return shoppingCart;
            }

            set => shoppingCart = value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Update the shopping cart values.
        ///     Async - Await is not necessary here as it is just an in memory list being updated.
        ///     The async - await is written to just mimic the backend service by
        ///     introducing artificial latency in finishing the task.
        /// </summary>
        /// <param name="updatedCart">Shopping cart entity</param>
        public async Task<ShoppingCart> Update(IShoppingCart updatedCart)
        {
            await Task.Run(() =>
            {
                InMemoryShoppingCart.ItemCount = updatedCart.ItemCount;
                InMemoryShoppingCart.TotalValue = updatedCart.TotalValue;
                InMemoryShoppingCart.ItemList = updatedCart.ItemList;
                InMemoryShoppingCart.ItemCountMap = updatedCart.ItemCountMap;
                Thread.Sleep(2000);
            });

            return InMemoryShoppingCart;
        }
    }
}