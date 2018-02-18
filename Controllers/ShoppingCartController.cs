﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Contracts;
using ShoppingCartWebApi.Controllers.HttpErrors;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.Controllers
{
    [Route("api/shoppingcart")]
    public class ShoppingCartController : IShoppingCartController
    {
        private readonly IShoppingCartRepository shoppingCartEntities;

        public ShoppingCartController(IShoppingCartRepository shoppingCartEntities)
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
            if (!DoesItemExist(itemId))
                return itemId.ErrorItemNotFound();

            await Task.Run(() =>
            {
                var itemsAlreadyInCart = shoppingCartEntities.InMemoryShoppingCart.ItemList.Where(x => x.Id == itemId)
                    .ToList();

                if (itemCount < itemsAlreadyInCart.Count)
                {
                    var numberOfItemsToDelete = itemsAlreadyInCart.Count - itemCount;
                    RemoveItemFromShoppingList(itemId, numberOfItemsToDelete);
                    ReduceCountInItemCountMap(itemId, numberOfItemsToDelete);
                }
                else
                {
                    var quantityToBeUpdated = itemCount - itemsAlreadyInCart.Count;
                    var item = itemsAlreadyInCart.FirstOrDefault(x => x.Id == itemId);
                    AddItemToShoppingList(item, quantityToBeUpdated);
                    UpdateItemCountInShoppingCart();
                }

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
                return itemId.ErrorItemNotFound();

            await Task.Run(() =>
            {
                var itemCount = shoppingCartEntities.InMemoryShoppingCart.ItemCountMap[itemId];
                RemoveItemFromShoppingList(itemId, itemCount);
                UpdateItemCountInShoppingCart(itemId);
                UpdateShoppingCartValues();
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
                RemoveAllItemFromShoppingCart();
                ClearItemCountInShoppingCart();
                UpdateShoppingCartValues();
            });

            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }

        private void ReduceCountInItemCountMap(int itemId,
            int numberOfItemsToDelete)
        {
            if (shoppingCartEntities.InMemoryShoppingCart.ItemList.Count == 0)
                shoppingCartEntities.InMemoryShoppingCart.ItemCountMap.Clear();
            else
                shoppingCartEntities.InMemoryShoppingCart.ItemCountMap[itemId] -= numberOfItemsToDelete;
        }

        private void AddItemToShoppingList(Item item)
        {
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

        private void AddItemToShoppingList(Item item, int quantity)
        {
            for (var count = 0; count < quantity; count++)
                shoppingCartEntities.InMemoryShoppingCart.ItemList.Add(item);
        }

        private void RemoveItemFromShoppingList(int itemId, int numberOfItemsToDelete)
        {
            var start = shoppingCartEntities.InMemoryShoppingCart.ItemCount;
            var deletedItemCount = 0;
            for (var count = start - 1; count >= 0; count--)
            {
                var item = shoppingCartEntities.InMemoryShoppingCart.ItemList[count];
                if (item.Id != itemId)
                    continue;

                shoppingCartEntities.InMemoryShoppingCart.ItemList.RemoveAt(count);
                deletedItemCount++;
                if (deletedItemCount == numberOfItemsToDelete)
                    break;
            }
        }

        private void UpdateItemCountInShoppingCart(int itemId)
        {
            shoppingCartEntities.InMemoryShoppingCart.UpdateItemCountMapUponDelete(itemId);
        }

        private void RemoveAllItemFromShoppingCart()
        {
            shoppingCartEntities.InMemoryShoppingCart.ItemList.Clear();
        }

        private void ClearItemCountInShoppingCart()
        {
            shoppingCartEntities.InMemoryShoppingCart.ClearItemCountMap();
        }
    }
}