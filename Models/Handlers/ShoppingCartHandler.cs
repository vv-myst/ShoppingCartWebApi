using System.Linq;
using System.Threading.Tasks;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi.Models.Handlers
{
    public class ShoppingCartHandler : IShoppingCartHandler
    {
        private readonly IShoppingCartRepository shoppingCartRepository;

        public ShoppingCartHandler(IShoppingCartRepository shoppingCartRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Check if an item with the specific itemId exists in the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be matched</param>
        /// <returns></returns>
        public bool DoesItemExist(ShoppingCart shoppingCart, int itemId)
        {
            return shoppingCart.ItemList.FirstOrDefault(x => x.Id == itemId) != null;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Adds an item to the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="item">Item to be added</param>
        public async Task<ShoppingCart> AddItemToShoppingCart(ShoppingCart shoppingCart, Item item)
        {
            AddItemToItemList(shoppingCart, item);
            UpdateItemCountMapUponAdd(shoppingCart);
            UpdateShoppingCartValues(shoppingCart);
            return await shoppingCartRepository.Update(shoppingCart);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Delete all items from the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be deleted</param>
        public async Task<ShoppingCart> DeleteItemsFromShoppingCart(ShoppingCart shoppingCart, int itemId)
        {
            var countOfItemInList = shoppingCart.ItemList.Count(item => item.Id == itemId);

            if (countOfItemInList == 0) 
                return null;

            RemoveItemFromItemList(shoppingCart, itemId, countOfItemInList);
            UpdateItemCountMapUponDelete(shoppingCart, itemId);
            UpdateShoppingCartValues(shoppingCart);
            return await shoppingCartRepository.Update(shoppingCart);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Update the quantity of items in the shopping cart for the given itemId
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        /// <param name="itemId">ItemId of the item to be updated</param>
        /// <param name="itemCount">Quantity of the item to be updated</param>
        public async Task<ShoppingCart> UpdateItemQuantityInShoppingCart(ShoppingCart shoppingCart, int itemId,
            int itemCount)
        {
            var itemsAlreadyInCart = shoppingCart.ItemList.Where(x => x.Id == itemId)
                .ToList();

            if (itemCount < itemsAlreadyInCart.Count)
            {
                var numberOfItemsToDelete = itemsAlreadyInCart.Count - itemCount;
                RemoveItemFromItemList(shoppingCart, itemId, numberOfItemsToDelete);
                ReduceCountInItemCountMap(shoppingCart, itemId, numberOfItemsToDelete);
            }
            else
            {
                var quantityToBeUpdated = itemCount - itemsAlreadyInCart.Count;
                var item = itemsAlreadyInCart.FirstOrDefault(x => x.Id == itemId);
                AddItemToItemList(shoppingCart, item, quantityToBeUpdated);
                UpdateItemCountMapUponAdd(shoppingCart);
            }

            UpdateShoppingCartValues(shoppingCart);
            return await shoppingCartRepository.Update(shoppingCart);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Remove all items from the shopping cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart entity</param>
        public void EmptyShoppingCart(ShoppingCart shoppingCart)
        {
            RemoveAllItemFromItemList(shoppingCart);
            RemoveAllItemsFromItemCountMap(shoppingCart);
            UpdateShoppingCartValues(shoppingCart);
        }

        private static void AddItemToItemList(ShoppingCart shoppingCart, Item item)
        {
            shoppingCart.ItemList.Add(item);
        }

        private static void UpdateItemCountMapUponAdd(ShoppingCart shoppingCart)
        {
            foreach (var item in shoppingCart.ItemList)
                shoppingCart.ItemCountMap[item.Id] = shoppingCart.ItemList.Count(x => x.Id == item.Id);
        }

        private static void UpdateItemCountMapUponDelete(ShoppingCart shoppingCart, int itemId)
        {
            if (shoppingCart.ItemList.Count == 0)
                shoppingCart.ItemCountMap.Clear();
            else
                shoppingCart.ItemCountMap[itemId] = 0;
        }

        private static void UpdateShoppingCartValues(ShoppingCart shoppingCart)
        {
            shoppingCart.ItemCount = shoppingCart.ItemList.Count;
            shoppingCart.TotalValue = shoppingCart.ItemList.Sum(item => item.Value);
        }

        private static void AddItemToItemList(ShoppingCart shoppingCart, Item item, int quantity)
        {
            for (var count = 0; count < quantity; count++)
                shoppingCart.ItemList.Add(item);
        }

        private static void ReduceCountInItemCountMap(ShoppingCart shoppingCart, int itemId,
            int numberOfItemsToDelete)
        {
            if (shoppingCart.ItemList.Count == 0)
                shoppingCart.ItemCountMap.Clear();
            else
                shoppingCart.ItemCountMap[itemId] -= numberOfItemsToDelete;
        }

        private static void RemoveItemFromItemList(ShoppingCart shoppingCart, int itemId, int numberOfItemsToDelete)
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
        }

        private static void RemoveAllItemFromItemList(ShoppingCart shoppingCart)
        {
            shoppingCart.ItemList.Clear();
        }

        private static void RemoveAllItemsFromItemCountMap(ShoppingCart shoppingCart)
        {
            shoppingCart.ItemCountMap.Clear();
        }
    }
}