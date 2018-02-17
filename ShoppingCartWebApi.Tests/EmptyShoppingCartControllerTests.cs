using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ShoppingCartWebApi;
using ShoppingCartWebApi.Controllers;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class EmptyShoppingCartControllerTests
    {
        [SetUp]
        public async Task Init()
        {
            mockItem1 = new Mock<Item>(100, "Dummy Item 1", 100.00M, 10, "Dummy Item 1").Object;
            mockItem2 = new Mock<Item>(200, "Dummy Item 2", 50.00M, 20, "Dummy Item 2").Object;
            mockShoppingCartEntities = new Mock<ShoppingCartEntities>().Object;
            shoppingCartController = new ShoppingCartController(mockShoppingCartEntities);
            emptyShoppingCartController = new EmptyShoppingCartController(mockShoppingCartEntities);
            await AddMockItems();
        }
        
        private Item mockItem1;
        private Item mockItem2;
        private ShoppingCartEntities mockShoppingCartEntities;
        private ShoppingCartController shoppingCartController;
        private EmptyShoppingCartController emptyShoppingCartController;


        private async Task AddMockItems()
        {
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            await shoppingCartController.Post(mockItem1, mockItem1.Id);
            await shoppingCartController.Post(mockItem2, mockItem2.Id);
        }

        [Test]
        public async Task Delete_RemoveAllItems_ClearsShoppingCart()
        {
            var result = (ObjectResult) await emptyShoppingCartController.Delete();
            var resultCart = result.Value as ShoppingCart;
            
            //Test in memory shopping cart 
            Assert.That(resultCart.ItemCount, Is.EqualTo(0));
            Assert.That(resultCart.ItemList, Is.Empty);
            Assert.That(resultCart.TotalValue, Is.EqualTo(0.00M));
            Assert.That(resultCart.ItemCountMap, Is.Empty);

            //Test api response
            Assert.That(result.StatusCode, Is.EqualTo((int) HttpStatusCode.OK));
        }
    }
}