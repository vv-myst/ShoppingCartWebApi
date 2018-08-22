using Moq;
using NUnit.Framework;
using ShoppingCartWebApi;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Handlers;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class ShoppingCartHandlerTests : TestSetup
    {
        [SetUp]
        public void Initialize()
        {
            mockShoppingCartRepository = new Mock<ShoppingCartRepository>().Object;
            mockShoppingCartHandler = new Mock<ShoppingCartHandler>(mockShoppingCartRepository).Object;
        }

        private IShoppingCartRepository mockShoppingCartRepository;
        private IShoppingCartHandler mockShoppingCartHandler;

        [Test]
        public void AddItemToShoppingCart_AddsItemToList_UpdatesShoppingCartValues()
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);

            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(100.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));
        }
        
        [TestCase(4)]
        public void UpdateItemQuantityIncrease_AddsItemToList_UpdatesItemCountMap_UpdatesShoppingCartValues(
            int itemCount)
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem2);

            //increase quantity of MockItem2 in ItemList to increaseItemCount value
            mockShoppingCartHandler.UpdateItemQuantityInShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart,
                MockItem2.Id, itemCount);

            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(5));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(5));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(300.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem2.Id], Is.EqualTo(4));
        }

        [TestCase(5, 3)]
        public void UpdateItemQuantityDecrease_RemovesItemsFromList_UpdatesItemCountMap_UpdatesShoppingCartValues(
            int increaseItemCount, int decreaseItemCount)
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem2);

            //increase quantity of MockItem2 in ItemList to increaseItemCount value
            mockShoppingCartHandler.UpdateItemQuantityInShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart,
                MockItem2.Id, increaseItemCount);

            //decrease quantity of MockItem 2 in ItemList to decreaseItemCount Value
            mockShoppingCartHandler.UpdateItemQuantityInShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart,
                MockItem2.Id, decreaseItemCount);

            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(4));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(4));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(250.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem2.Id], Is.EqualTo(3));
        }

        [TestCase(5)]
        public void DeleteItems_RemovesAllItemsOfGivenIdFromList_UpdatesShoppingCartValues(int itemCount)
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem2);

            //update the quantity of MockItem2 items in list to the given itemCount 
            mockShoppingCartHandler.UpdateItemQuantityInShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart,
                MockItem2.Id, itemCount);

            //delete all MockItem2 items from the list.
            mockShoppingCartHandler.DeleteItemsFromShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart,
                MockItem2.Id);

            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem2), Is.False);
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(100.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[MockItem2.Id], Is.EqualTo(0));
        }

        [TestCase(5)]
        public void DeleteAll_RemovesAllItemsFromList_UpdatesShoppingCartValues(int itemCount)
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem2);

            //update the quantity of MockItem2 items in list to the given itemCount 
            mockShoppingCartHandler.UpdateItemQuantityInShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart,
                MockItem2.Id, itemCount);

            //delete all items from the list.
            mockShoppingCartHandler.EmptyShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart);

            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(0));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem1), Is.False);
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(MockItem2), Is.False);
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(0));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(0.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(0));
        }

        [Test]
        public void CheckIfItemExist_FalseIfItemNotInList()
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);

            var result =
                mockShoppingCartHandler.DoesItemExist(mockShoppingCartRepository.InMemoryShoppingCart, MockItem2.Id);

            Assert.That(result, Is.False);
        }

        [Test]
        public void CheckIfItemExist_TrueIfItemInList()
        {
            //add items to Shopping cart
            mockShoppingCartHandler.AddItemToShoppingCart(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1);

            var result =
                mockShoppingCartHandler.DoesItemExist(mockShoppingCartRepository.InMemoryShoppingCart, MockItem1.Id);

            Assert.That(result, Is.True);
        }
    }
}