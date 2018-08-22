using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShoppingCartWebApi;
using ShoppingCartWebApi.Contracts;
using ShoppingCartWebApi.Controllers;
using ShoppingCartWebApi.Controllers.HttpErrors;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Handlers;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class ShoppingCartControllerTests : TestSetup
    {
        [SetUp]
        public void Initialize()
        {
            mockShoppingCartRepository = new Mock<ShoppingCartRepository>().Object;
            mockShoppingCartHandler = new Mock<ShoppingCartHandler>(mockShoppingCartRepository).Object;
            mockShoppingCartController =
                new Mock<ShoppingCartController>(mockShoppingCartRepository, mockShoppingCartHandler).Object;
        }

        private IShoppingCartRepository mockShoppingCartRepository;
        private IShoppingCartController mockShoppingCartController;
        private IShoppingCartHandler mockShoppingCartHandler;

        [Test]
        public async Task Post_AddDifferentItem_AddsToShoppingCartAddsANewItem()
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            var result = (ObjectResult) await mockShoppingCartController.Post(MockItem2, MockItem2.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(2));
            Assert.That(resultCart.ItemList.Contains(MockItem1));
            Assert.That(resultCart.ItemList.Contains(MockItem2));
            Assert.That(resultCart.TotalValue, Is.EqualTo(150.00M));
            Assert.That(resultCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));
            Assert.That(resultCart.ItemCountMap[MockItem2.Id], Is.EqualTo(1));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }

        [Test]
        public async Task Post_AddsItemToTheShoppingCart()
        {
            var result = (ObjectResult) await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(1));
            Assert.That(resultCart.ItemList.Contains(MockItem1));
            Assert.That(resultCart.TotalValue, Is.EqualTo(MockItem1.Value));
            Assert.That(resultCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));

            var item = resultCart.ItemList[0];
            Assert.That(item.Id, Is.EqualTo(MockItem1.Id));
            Assert.That(item.Name, Is.EqualTo(MockItem1.Name));
            Assert.That(item.Description, Is.EqualTo(MockItem1.Description));
            Assert.That(item.Value, Is.EqualTo(MockItem1.Value));
            Assert.That(item.InventoryCount, Is.EqualTo(MockItem1.InventoryCount));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }

        [Test]
        public async Task Post_SameItemAddedAgain_UpatesItemCountMapAndAddsToItemList()
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            var result = (ObjectResult) await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(2));
            Assert.That(resultCart.ItemList.Contains(MockItem1));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(2));
            Assert.That(resultCart.TotalValue, Is.EqualTo(200.00M));
            Assert.That(resultCart.ItemCountMap[MockItem1.Id], Is.EqualTo(2));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
        
        [TestCase(1000)]
        public async Task Post_DifferentItemIdInUrlAndObject_Throws400Error(int itemId)
        {
            var result = (ObjectResult) await mockShoppingCartController.Post(MockItem1, itemId);
            var resultString = result.Value as string;
            var actualErrorMsg = ((ObjectResult) MockItem1.ErrorItemIdsDoNotMatch(itemId)).Value as string;

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
            Assert.That(resultString, Is.EqualTo(actualErrorMsg));
        }

        [TestCase(5)]
        public async Task Put_IncreaseItemQuantity_UpdatesItemCountMapAndAddsToItemList(int itemCount)
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            var result = (ObjectResult) await mockShoppingCartController.Put(MockItem1.Id, itemCount);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(5));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(5));
            Assert.That(resultCart.TotalValue, Is.EqualTo(500.00M));
            Assert.That(resultCart.ItemCountMap[MockItem1.Id], Is.EqualTo(5));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }

        [TestCase(7, 3)]
        public async Task Put_DecreaseItemQuantity_UpdatesItemCountMapAndAddsToItemList(int increasedItemCount,
            int reducedItemCount)
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Put(MockItem1.Id, increasedItemCount);
            var result = (ObjectResult) await mockShoppingCartController.Put(MockItem1.Id, reducedItemCount);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(3));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(3));
            Assert.That(resultCart.TotalValue, Is.EqualTo(300.00M));
            Assert.That(resultCart.ItemCountMap[MockItem1.Id], Is.EqualTo(3));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }

        [TestCase(300, 5)]
        public async Task Put_ItemIdNotInItemList_Throws400Error(int itemId, int itemCount)
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            var result = (ObjectResult) await mockShoppingCartController.Put(itemId, itemCount);
            var resultString = result.Value as string;
            var actualErrorMsg = ((ObjectResult) itemId.ErrorItemNotFound()).Value as string;

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
            Assert.That(resultString, Is.EqualTo(actualErrorMsg));
        }

        [TestCase(300)]
        public async Task Delete_ItemIdNotInItemList_Throws400Error(int itemId)
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Post(MockItem2, MockItem2.Id);
            var result = (ObjectResult) await mockShoppingCartController.Delete(itemId);
            var resultString = result.Value as string;
            var actualErrorMsg = ((ObjectResult) itemId.ErrorItemNotFound()).Value as string;

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
            Assert.That(resultString, Is.EqualTo(actualErrorMsg));
        }

        [Test]
        public async Task Delete_DeleteItemFromList_RemovesItemAndUpdatesItemCountMap()
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Post(MockItem2, MockItem2.Id);
            var result = (ObjectResult) await mockShoppingCartController.Delete(MockItem1.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(1));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(1));
            Assert.That(resultCart.TotalValue, Is.EqualTo(50.00M));
            Assert.That(resultCart.ItemCountMap[MockItem1.Id], Is.EqualTo(0));
            Assert.That(resultCart.ItemCountMap[MockItem2.Id], Is.EqualTo(1));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }

        [Test]
        public async Task Delete_RemoveAllItems_ClearsShoppingCart()
        {
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Post(MockItem1, MockItem1.Id);
            await mockShoppingCartController.Post(MockItem2, MockItem2.Id);
            var result = (ObjectResult) await mockShoppingCartController.Delete();
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart, Is.Not.Null);
            Assert.That(resultCart.ItemCount, Is.EqualTo(0));
            Assert.That(resultCart.ItemList, Is.Empty);
            Assert.That(resultCart.TotalValue, Is.EqualTo(0.00M));
            Assert.That(resultCart.ItemCountMap, Is.Empty);

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
    }
}