using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Contracts;
using ShoppingCartWebApi.Controllers.Extensions;
using ShoppingCartWebApi.Controllers.HttpErrors;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.Controllers
{
    [Route("api/shoppingcart")]
    public class ShoppingCartController : IShoppingCartController
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Add a new item to the shopping cart
        ///     path - /shoppingcart/{itemId}
        /// </summary>
        /// <param name="item">Item to be added to the shopping cart</param>
        /// <param name="itemId">ItemId received in the request URL</param>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200 if success
        ///     else returns an Http ObjectResult with Status code 400
        /// </returns>
        [HttpPost("{itemid}")]
        public async Task<IActionResult> Post([FromBody] Item item, int itemId)
        {
            if (item.Id != itemId)
                return item.ErrorItemIdsDoNotMatch(itemId);

            var shoppingCart = shoppingCartRepository.InMemoryShoppingCart.AddItemToShoppingCart(item);
            var updatedCart = await shoppingCartRepository.Update(shoppingCart);

            return new ObjectResult(updatedCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Update the item count of a particular item in the shopping cart
        ///     path - /shoppingcart/{itemId}
        /// </summary>
        /// <param name="itemId">ItemId received in the request URL</param>
        /// <param name="itemCount">ItemCount received in the request URL</param>
        /// Returns Http ObjectResult with Status code 200 if success
        /// else returns an Http ObjectResult with Status code 400
        /// <returns />
        [HttpPut("{itemid}/{itemcount}")]
        public async Task<IActionResult> Put(int itemId, int itemCount)
        {
            if (!shoppingCartRepository.InMemoryShoppingCart.DoesItemExist(itemId))
                return itemId.ErrorItemNotFound();

            var shoppingCart =
                shoppingCartRepository.InMemoryShoppingCart.UpdateItemQuantityInShoppingCart(itemId, itemCount);

            var updatedCart = await shoppingCartRepository.Update(shoppingCart);

            return new ObjectResult(updatedCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Delete specific items from the shopping cart
        ///     path - /shoppingcart/{itemId}
        /// </summary>
        /// <param name="itemId">ItemId received in the request URL</param>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200 if success
        ///     else returns an Http ObjectResult with Status code 400
        /// </returns>
        [HttpDelete("{itemid}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            if (!shoppingCartRepository.InMemoryShoppingCart.DoesItemExist(itemId))
                return itemId.ErrorItemNotFound();

            var shoppingCart = shoppingCartRepository.InMemoryShoppingCart.DeleteItemsFromShoppingCart(itemId);
            var updatedCart = await shoppingCartRepository.Update(shoppingCart);

            return new ObjectResult(updatedCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Delete all the items in the shopping cart
        ///     path - /shoppingcart
        /// </summary>
        /// <returns>
        ///     Returns Http ObjectResult with Status code 200
        /// </returns>
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var shoppingCart = shoppingCartRepository.InMemoryShoppingCart.EmptyShoppingCart();
            var updatedCart = await shoppingCartRepository.Update(shoppingCart);

            return new ObjectResult(updatedCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }
    }
}