using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ShoppingCartWebApi;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class ShoppingCartRepositoryTests
    {
        [SetUp]
        public void Init()
        {
            mockItem1 = new Mock<Item>(100, "Dummy Item 1", 100.00M, 10, "Dummy Item 1").Object;
            mockItem2 = new Mock<Item>(200, "Dummy Item 2", 50.00M, 20, "Dummy Item 2").Object;
            mockShoppingCartRepository = new Mock<ShoppingCartRepository>().Object;
            mockShoppingCart1 = new Mock<ShoppingCart>(new List<Item>{mockItem1, mockItem2}).Object;
            mockShoppingCart2 = new Mock<ShoppingCart>(new List<Item>{mockItem1, mockItem2, mockItem1, mockItem2}).Object;
        }

        private Item mockItem1;
        private Item mockItem2;
        private IShoppingCartRepository mockShoppingCartRepository;
        private ShoppingCart mockShoppingCart1;
        private ShoppingCart mockShoppingCart2;

        [Test]
        public void Update_UpdatesInMemoryShoppingCartValues()
        {
            mockShoppingCartRepository.Update(mockShoppingCart1);

            //after Update with mockShoppingCart1
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(mockItem1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(mockItem2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(150.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[mockItem1.Id], Is.EqualTo(1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[mockItem2.Id], Is.EqualTo(1));

            mockShoppingCartRepository.Update(mockShoppingCart2);

            //after Update with mockShoppingCart2
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCount, Is.EqualTo(4));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(mockItem1));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Contains(mockItem2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(4));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.TotalValue, Is.EqualTo(300.00M));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[mockItem1.Id], Is.EqualTo(2));
            Assert.That(mockShoppingCartRepository.InMemoryShoppingCart.ItemCountMap[mockItem2.Id], Is.EqualTo(2));
        }
    }
}