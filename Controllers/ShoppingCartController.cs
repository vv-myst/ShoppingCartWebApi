#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi ShoppingCartController.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-16 8:45 
// 2018-02-15 10:57 

#endregion

using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Contracts;
using ShoppingCartWebApi.Controllers.HttpErrors;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.Controllers
{
    [Route("api/shoppingcart")]
    public class ShoppingCartController : IShoppingCartController
    {
        private readonly ShoppingCartEntities shoppingCartEntities;

        public ShoppingCartController(ShoppingCartEntities shoppingCartEntities)
        {
            this.shoppingCartEntities = shoppingCartEntities;
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
                AddItemToShoppingList(item);
                UpdateItemCountInShoppingCart();
                UpdateShoppingCartValues();
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int)HttpStatusCode.OK
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
        [HttpPut("{itemid}")]
        public async Task<IActionResult> Put(int itemId, [FromQuery] int itemCount)
        {
            if (!DoesItemExist(itemId))
                return itemId.ErrorNoItemFound();

            await Task.Run(() =>
            {
                var itemsAlreadyInCart = shoppingCartEntities.InMemoryShoppingCart.ItemList.Where(x => x.Id == itemId)
                    .ToList();
                var item = itemsAlreadyInCart.FirstOrDefault(x => x.Id == itemId);
                var quantityToBeUpdated = itemCount - itemsAlreadyInCart.Count;
                AddItemToShoppingList(item, quantityToBeUpdated);
                UpdateItemCountInShoppingCart();
                UpdateShoppingCartValues();
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
            if (!DoesItemExist(itemId))
                return itemId.ErrorNoItemFound();

            await Task.Run(() =>
            {
                RemoveItemFromShoppingList(itemId);
                UpdateItemCountInShoppingCart(itemId);
                UpdateShoppingCartValues();
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        private void AddItemToShoppingList(Item item)
        {
            shoppingCartEntities.InMemoryShoppingCart.ItemList.Add(item);
        }

        private void AddItemToShoppingList(Item item, int quantity)
        {
            for (var count = 0; count < quantity; count++)
                shoppingCartEntities.InMemoryShoppingCart.ItemList.Add(item);
        }

        private void UpdateItemCountInShoppingCart()
        {
            shoppingCartEntities.InMemoryShoppingCart.UpdateItemCountMapUponAdd();
        }

        private void UpdateShoppingCartValues()
        {
            shoppingCartEntities.InMemoryShoppingCart.UpdateShoppingCart();
        }

        private bool DoesItemExist(int itemId)
        {
            return shoppingCartEntities.InMemoryShoppingCart.ItemList.FirstOrDefault(x => x.Id == itemId) != null;
        }

        private void RemoveItemFromShoppingList(int itemId)
        {
            var start = shoppingCartEntities.InMemoryShoppingCart.ItemCount;
            for (var count = start - 1; count >= 0; count--)
            {
                var item = shoppingCartEntities.InMemoryShoppingCart.ItemList[count];
                if (item.Id == itemId)
                    shoppingCartEntities.InMemoryShoppingCart.ItemList.RemoveAt(count);
            }
        }

        private void UpdateItemCountInShoppingCart(int itemId)
        {
            shoppingCartEntities.InMemoryShoppingCart.UpdateItemCountMapUponDelete(itemId);
        }
    }
}