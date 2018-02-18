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
        ///     Update an existing entry in the repository
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        void Update(ShoppingCart entity);
    }
}