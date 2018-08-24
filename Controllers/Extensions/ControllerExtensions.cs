using System.Linq;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.Controllers.Extensions
{
    public static class ControllerExtensions
    {
        /// <summary>
        ///     Check if an item with the specific itemId exists in the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be matched</param>
        /// <returns></returns>
        public static bool DoesItemExist(this ShoppingCart shoppingCart, int itemId)
        {
            return shoppingCart.ItemList.FirstOrDefault(x => x.Id == itemId) != null;
        }

        /// <summary>
        ///     Adds an item to the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="item">Item to be added</param>
        public static ShoppingCart AddItemToShoppingCart(this ShoppingCart shoppingCart, IItem item)
        {
            var cart = shoppingCart.AddItemToItemList(item);
            cart = cart.UpdateItemCountMapUponAdd();
            cart = cart.UpdateShoppingCartValues();
            return cart;
        }

        private static ShoppingCart AddItemToItemList(this ShoppingCart shoppingCart, IItem item)
        {
            shoppingCart.ItemList.Add(item);
            return shoppingCart;
        }

        private static ShoppingCart UpdateItemCountMapUponAdd(this ShoppingCart shoppingCart)
        {
            foreach (var item in shoppingCart.ItemList)
                shoppingCart.ItemCountMap[item.Id] = shoppingCart.ItemList.Count(x => x.Id == item.Id);

            return shoppingCart;
        }

        private static ShoppingCart UpdateShoppingCartValues(this ShoppingCart shoppingCart)
        {
            shoppingCart.ItemCount = shoppingCart.ItemList.Count;
            shoppingCart.TotalValue = shoppingCart.ItemList.Sum(item => item.Value);

            return shoppingCart;
        }

        /// <summary>
        ///     Delete all items from the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be deleted</param>
        public static ShoppingCart DeleteItemsFromShoppingCart(this ShoppingCart shoppingCart, int itemId)
        {
            var countOfItemInList = shoppingCart.ItemList.Count(item => item.Id == itemId);

            if (countOfItemInList == 0)
                return null;

            var cart = shoppingCart.RemoveItemFromItemList(itemId, countOfItemInList);
            cart = cart.UpdateItemCountMapUponDelete(itemId);
            cart = cart.UpdateShoppingCartValues();

            return cart;
        }

        private static ShoppingCart RemoveItemFromItemList(this ShoppingCart shoppingCart, int itemId,
            int numberOfItemsToDelete)
        {
            var start = shoppingCart.ItemCount;
            var deletedItemCount = 0;


            for (var count = start - 1; count >= 0; count--)
            {
                var item = shoppingCart.ItemList[count];
                if (item.Id != itemId)
                    continue;

                shoppingCart.ItemList.RemoveAt(count);
                deletedItemCount++;
                if (deletedItemCount == numberOfItemsToDelete)
                    break;
            }

            return shoppingCart;
        }

        private static ShoppingCart UpdateItemCountMapUponDelete(this ShoppingCart shoppingCart, int itemId)
        {
            if (shoppingCart.ItemList.Count == 0)
                shoppingCart.ItemCountMap.Clear();
            else
                shoppingCart.ItemCountMap[itemId] = 0;

            return shoppingCart;
        }

        /// <summary>
        ///     Update the quantity of items in the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be updated</param>
        /// <param name="itemCount">Quantity of the item to be updated</param>
        public static ShoppingCart UpdateItemQuantityInShoppingCart(this ShoppingCart shoppingCart, int itemId,
            int itemCount)
        {
            var cart = shoppingCart;

            var itemsAlreadyInCart = shoppingCart.ItemList.Where(x => x.Id == itemId)
                .ToList();

            if (itemCount < itemsAlreadyInCart.Count)
            {
                var numberOfItemsToDelete = itemsAlreadyInCart.Count - itemCount;
                cart = cart.RemoveItemFromItemList(itemId, numberOfItemsToDelete);
                cart = cart.ReduceCountInItemCountMap(itemId, numberOfItemsToDelete);
            }
            else
            {
                var quantityToBeUpdated = itemCount - itemsAlreadyInCart.Count;
                var item = itemsAlreadyInCart.FirstOrDefault(x => x.Id == itemId);
                cart = cart.AddItemToItemList(item, quantityToBeUpdated);
                cart = cart.UpdateItemCountMapUponAdd();
            }

            cart = cart.UpdateShoppingCartValues();
            return cart;
        }

        private static ShoppingCart ReduceCountInItemCountMap(this ShoppingCart shoppingCart, int itemId,
            int numberOfItemsToDelete)
        {
            if (shoppingCart.ItemList.Count == 0)
                shoppingCart.ItemCountMap.Clear();
            else
                shoppingCart.ItemCountMap[itemId] -= numberOfItemsToDelete;

            return shoppingCart;
        }

        private static ShoppingCart AddItemToItemList(this ShoppingCart shoppingCart, IItem item, int quantity)
        {
            for (var count = 0; count < quantity; count++)
                shoppingCart.ItemList.Add(item);

            return shoppingCart;
        }

        /// <summary>
        ///     Remove all items from the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        public static ShoppingCart EmptyShoppingCart(this ShoppingCart shoppingCart)
        {
            var cart = shoppingCart.RemoveAllItemFromItemList();
            cart = cart.RemoveAllItemsFromItemCountMap();
            cart = cart.UpdateShoppingCartValues();
            return cart;
        }

        private static ShoppingCart RemoveAllItemFromItemList(this ShoppingCart shoppingCart)
        {
            shoppingCart.ItemList.Clear();
            return shoppingCart;
        }

        private static ShoppingCart RemoveAllItemsFromItemCountMap(this ShoppingCart shoppingCart)
        {
            shoppingCart.ItemCountMap.Clear();
            return shoppingCart;
        }
    }
}