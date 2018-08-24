using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class ShoppingCartRepositoryTests : TestSetup
    {
        [SetUp]
        public void Initialize()
        {
            sut = new ShoppingCartRepository();

            var fixture = new Fixture();

            mockShoppingCart1 = fixture.Build<ShoppingCart>()
                .With(x => x.ItemList, new List<IItem> {MockItem1, MockItem2}).Without(x => x.ItemCount)
                .Without(x => x.TotalValue).Without(x => x.ItemCountMap).Create();

            mockShoppingCart2 = fixture.Build<ShoppingCart>()
                .With(x => x.ItemList, new List<IItem> {MockItem1, MockItem2, MockItem1, MockItem2})
                .Without(x => x.ItemCount)
                .Without(x => x.TotalValue).Without(x => x.ItemCountMap).Create();
        }

        private IShoppingCartRepository sut;
        private IShoppingCart mockShoppingCart1;
        private IShoppingCart mockShoppingCart2;

        [Test]
        public async Task Update_UpdatesInMemoryShoppingCartValues()
        {
            await sut.Update(mockShoppingCart1);

            //after Update with mockShoppingCart1
            Assert.That(sut.InMemoryShoppingCart.ItemCount, Is.EqualTo(2));
            Assert.That(sut.InMemoryShoppingCart.ItemList.Contains(MockItem1));
            Assert.That(sut.InMemoryShoppingCart.ItemList.Contains(MockItem2));
            Assert.That(sut.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(2));
            Assert.That(sut.InMemoryShoppingCart.TotalValue, Is.EqualTo(150.00M));
            Assert.That(sut.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(sut.InMemoryShoppingCart.ItemCountMap[MockItem1.Id], Is.EqualTo(1));
            Assert.That(sut.InMemoryShoppingCart.ItemCountMap[MockItem2.Id], Is.EqualTo(1));

            await sut.Update(mockShoppingCart2);

            //after Update with mockShoppingCart2
            Assert.That(sut.InMemoryShoppingCart.ItemCount, Is.EqualTo(4));
            Assert.That(sut.InMemoryShoppingCart.ItemList.Contains(MockItem1));
            Assert.That(sut.InMemoryShoppingCart.ItemList.Contains(MockItem2));
            Assert.That(sut.InMemoryShoppingCart.ItemList.Count, Is.EqualTo(4));
            Assert.That(sut.InMemoryShoppingCart.TotalValue, Is.EqualTo(300.00M));
            Assert.That(sut.InMemoryShoppingCart.ItemCountMap.Count, Is.EqualTo(2));
            Assert.That(sut.InMemoryShoppingCart.ItemCountMap[MockItem1.Id], Is.EqualTo(2));
            Assert.That(sut.InMemoryShoppingCart.ItemCountMap[MockItem2.Id], Is.EqualTo(2));
        }
    }
}