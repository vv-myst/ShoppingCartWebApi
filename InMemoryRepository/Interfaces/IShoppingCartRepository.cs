using System.Threading.Tasks;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.InMemoryRepository.Interfaces
{
    public interface IShoppingCartRepository
    {
        /// <summary>
        ///     An in memory storage of Shopping Cart and its changes
        /// </summary>
        ShoppingCart InMemoryShoppingCart { get; }

        /// <summary>
        ///     Update the shopping cart values
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        Task<ShoppingCart> Update(ShoppingCart shoppingCart);
    }
}