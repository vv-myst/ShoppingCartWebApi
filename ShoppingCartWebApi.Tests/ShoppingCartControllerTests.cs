using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShoppingCartWebApi;
using ShoppingCartWebApi.Controllers;
using ShoppingCartWebApi.Controllers.HttpErrors;
using ShoppingCartWebApi.Models;
using Assert = NUnit.Framework.Assert;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class ShoppingCartControllerTests
    {
        [SetUp]
        public void Init()
        {
            mockItem1 = new Mock<Item>(100, "Dummy Item 1", 100.00M, 10, "Dummy Item 1").Object;
            mockItem2 = new Mock<Item>(200, "Dummy Item 2", 50.00M, 20, "Dummy Item 2").Object;
            mockShoppingCartEntities = new Mock<ShoppingCartEntities>().Object;
            shoppingCartController = new ShoppingCartController(mockShoppingCartEntities);
        }
        
        private Item mockItem1;
        private Item mockItem2;
        private ShoppingCartEntities mockShoppingCartEntities;
        private ShoppingCartController shoppingCartController;

        [Test]
        public async Task Post_AddsItemToTheShoppingCart()
        {
            var result = (ObjectResult) await shoppingCartController.Post(mockItem1, mockItem1.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart.ItemCount, Is.EqualTo(1));
            Assert.That(resultCart.ItemList.Contains(mockItem1));
            Assert.That(resultCart.TotalValue, Is.EqualTo(mockItem1.Value));
            Assert.That(resultCart.ItemCountMap[mockItem1.Id], Is.EqualTo(1));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
        
        [Test]
        public async Task Post_SameItemAddedAgain_UpatesItemCountMapAndAddsToItemList()
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            var result = (ObjectResult) await shoppingCartController.Post(mockItem1, mockItem1.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart.ItemCount, Is.EqualTo(2));
            Assert.That(resultCart.ItemList.Contains(mockItem1));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(2));
            Assert.That(resultCart.TotalValue, Is.EqualTo(200.00M));
            Assert.That(resultCart.ItemCountMap[mockItem1.Id], Is.EqualTo(2));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
        
        [Test]
        public async Task Post_AddDifferentItem_AddsToShoppingCartAddsANewItem()
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            var result = (ObjectResult) await shoppingCartController.Post(mockItem2, mockItem2.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart.ItemCount, Is.EqualTo(2));
            Assert.That(resultCart.ItemList.Contains(mockItem1));
            Assert.That(resultCart.ItemList.Contains(mockItem2));
            Assert.That(resultCart.TotalValue, Is.EqualTo(150.00M));
            Assert.That(resultCart.ItemCountMap[mockItem1.Id], Is.EqualTo(1));
            Assert.That(resultCart.ItemCountMap[mockItem2.Id], Is.EqualTo(1));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
        
        [TestCase(1000)]
        public async Task Post_DifferentItemIdInUrlAndObject_Throws400Error(int itemId)
        {
            var result = (ObjectResult) await shoppingCartController.Post(mockItem1, itemId);
            var resultString = result.Value as string;
            var actualErrorMsg = ((ObjectResult) mockItem1.ErrorItemIdsDoNotMatch(itemId)).Value as string;
            
            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
            Assert.That(resultString, Is.EqualTo(actualErrorMsg));
        }
        
        [TestCase(5)]
        public async Task Put_UpdateItemQuantity_UpdatesItemCountMapAndAddsToItemList(int itemCount)
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            var result = (ObjectResult) await shoppingCartController.Put(mockItem1.Id, itemCount);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart.ItemCount, Is.EqualTo(5));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(5));
            Assert.That(resultCart.TotalValue, Is.EqualTo(500.00M));
            Assert.That(resultCart.ItemCountMap[mockItem1.Id], Is.EqualTo(5));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
        
        [TestCase(300, 5)]
        public async Task Put_ItemIdNotInItemList_Throws400Error(int itemId, int itemCount)
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            var result = (ObjectResult) await shoppingCartController.Put(itemId, itemCount);
            var resultString = result.Value as string;
            var actualErrorMsg = ((ObjectResult)itemId.ErrorNoItemFound()).Value as string;
            
            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
            Assert.That(resultString, Is.EqualTo(actualErrorMsg));
        }
        
        [Test]
        public async Task Delete_DeleteItemFromList_RemovesItemAndUpdatesItemCountMap()
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            await shoppingCartController.Post(mockItem2, mockItem2.Id);
            var result = (ObjectResult) await shoppingCartController.Delete(mockItem1.Id);
            var resultCart = result.Value as ShoppingCart;

            //Test in memory shopping cart 
            Assert.That(resultCart.ItemCount, Is.EqualTo(1));
            Assert.That(resultCart.ItemList.Count, Is.EqualTo(1));
            Assert.That(resultCart.TotalValue, Is.EqualTo(50.00M));
            Assert.That(resultCart.ItemCountMap[mockItem1.Id], Is.EqualTo(0));
            Assert.That(resultCart.ItemCountMap[mockItem2.Id], Is.EqualTo(1));

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
        
        [TestCase(300)]
        public async Task Delete_ItemIdNotInItemList_Throws400Error(int itemId)
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            await shoppingCartController.Post(mockItem2, mockItem2.Id);
            var result = (ObjectResult) await shoppingCartController.Delete(itemId);
            var resultString = result.Value as string;
            var actualErrorMsg = ((ObjectResult)itemId.ErrorNoItemFound()).Value as string;
            
            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.BadRequest));
            Assert.That(resultString, Is.EqualTo(actualErrorMsg));
        }
    }
}