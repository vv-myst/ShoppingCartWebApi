using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Contracts;
using ShoppingCartWebApi.Controllers.HttpErrors;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.Controllers
{
    [Route("api/shoppingcart")]
    public class ShoppingCartController : IShoppingCartController
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IShoppingCartHandler shoppingCartHandler;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
            IShoppingCartHandler shoppingCartHandler)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.shoppingCartHandler = shoppingCartHandler;
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

            var shoppingCart =
                await shoppingCartHandler.AddItemToShoppingCart(shoppingCartRepository.InMemoryShoppingCart, item);

            return new ObjectResult(shoppingCart)
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
        ///     Returns Http ObjectResult with Status code 200 if success
        ///     else returns an Http ObjectResult with Status code 400 
        /// <returns/>
        [HttpPut("{itemid}/{itemcount}")]
        public async Task<IActionResult> Put(int itemId, int itemCount)
        {
            if (!shoppingCartHandler.DoesItemExist(shoppingCartRepository.InMemoryShoppingCart, itemId))
                return itemId.ErrorItemNotFound();

            var shoppingCart = await shoppingCartHandler.UpdateItemQuantityInShoppingCart(
                shoppingCartRepository.InMemoryShoppingCart, itemId,
                itemCount);
            
            return new ObjectResult(shoppingCart)
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
            if (!shoppingCartHandler.DoesItemExist(shoppingCartRepository.InMemoryShoppingCart, itemId))
                return itemId.ErrorItemNotFound();

            var shoppingCart =
                await shoppingCartHandler.DeleteItemsFromShoppingCart(shoppingCartRepository.InMemoryShoppingCart,
                    itemId);
            
            return new ObjectResult(shoppingCart)
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
            await Task.Run(() =>
            {
                shoppingCartHandler.EmptyShoppingCart(shoppingCartRepository.InMemoryShoppingCart);
            });

            return new ObjectResult(shoppingCartRepository.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }
    }
}