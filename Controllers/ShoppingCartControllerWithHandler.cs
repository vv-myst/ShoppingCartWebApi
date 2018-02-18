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
    public class ShoppingCartControllerWithHandler : IShoppingCartController
    {
        private readonly IShoppingCartRepository shoppingCartEntities;
        private readonly IShoppingCartHandler shoppingCartHandler;

        public ShoppingCartControllerWithHandler(IShoppingCartRepository shoppingCartEntities,
            IShoppingCartHandler shoppingCartHandler)
        {
            this.shoppingCartEntities = shoppingCartEntities;
            this.shoppingCartHandler = shoppingCartHandler;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Add a new item to the shopping cart
        ///     path - /shoppingcart/{itemId}
        /// </summary>
        /// <param name="item"></param>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpPost("{itemid}")]
        public async Task<IActionResult> Post([FromBody] Item item, int itemId)
        {
            if (item.Id != itemId)
                return item.ErrorItemIdsDoNotMatch(itemId);

            await Task.Run(() =>
            {
                shoppingCartHandler.AddItemToShoppingCart(shoppingCartEntities.InMemoryShoppingCart, item);
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Update the item count of a particular item in the shopping cart
        ///     path - /shoppingcart/{itemId}
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="itemCount"></param>
        /// <returns></returns>
        [HttpPut("{itemid}/{itemcount}")]
        public async Task<IActionResult> Put(int itemId, int itemCount)
        {
            if (!shoppingCartHandler.DoesItemExist(shoppingCartEntities.InMemoryShoppingCart, itemId))
                return itemId.ErrorItemNotFound();

            await Task.Run(() =>
            {
                shoppingCartHandler.UpdateItemQuantityInShoppingCart(shoppingCartEntities.InMemoryShoppingCart, itemId,
                    itemCount);
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Delete a specific item from the shopping cart
        ///     path - /shoppingcart/{itemId}
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        [HttpDelete("{itemid}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            if (!shoppingCartHandler.DoesItemExist(shoppingCartEntities.InMemoryShoppingCart, itemId))
                return itemId.ErrorItemNotFound();

            await Task.Run(() =>
            {
                shoppingCartHandler.DeleteItemsFromShoppingCart(shoppingCartEntities.InMemoryShoppingCart, itemId);
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        /// <inheritdoc />
        /// <summary>
        ///     Delete all the items in the shopping cart
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await Task.Run(() =>
            {
                shoppingCartHandler.EmptyShoppingCart(shoppingCartEntities.InMemoryShoppingCart);
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }
    }
}