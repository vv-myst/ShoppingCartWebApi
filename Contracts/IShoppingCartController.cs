using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.Contracts
{
    public interface IShoppingCartController
    {
        /// <summary>
        ///     Add a new item to the shopping cart
        /// </summary>
        /// <param name="item">Item to be added to the shopping cart</param>
        /// <param name="itemId">ItemId received in the request URL</param>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200 if success
        ///     else returns an Http ObjectResult with Status code 400 
        /// </returns>
        Task<IActionResult> Post([FromBody] Item item, int itemId);

        /// <summary>
        ///     Update the item count of a particular item in the shopping cart
        /// </summary>
        /// <param name="itemId">ItemId received in the request URL</param>
        /// <param name="itemCount">ItemCount received in the request URL</param>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200 if success
        ///     else returns an Http ObjectResult with Status code 400 
        /// </returns>
        Task<IActionResult> Put(int itemId, int itemCount);

        /// <summary>
        ///     Delete specific items from the shopping cart
        /// </summary>
        /// <param name="itemId">ItemId received in the request URL</param>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200 if success
        ///     else returns an Http ObjectResult with Status code 400 
        /// </returns>
        Task<IActionResult> Delete(int itemId);
        
        /// <summary>
        ///     Delete all the items in the shopping cart
        /// </summary>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200
        /// </returns>
        Task<IActionResult> Delete();
    }
}
