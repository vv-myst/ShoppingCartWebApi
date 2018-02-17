#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi IShoppingCartController.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-16 8:41 
// 2018-02-15 10:57 

#endregion

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
        /// <param name="item"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<IActionResult> Post([FromBody] Item item, int itemId);

        /// <summary>
        ///     Update the item count of a particular item in the shopping cart
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        Task<IActionResult> Put(int itemId, [FromQuery] int itemCount);

        /// <summary>
        ///     Delete a specific item from the shopping cart
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        Task<IActionResult> Delete(int itemId);
    }
}
