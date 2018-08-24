using AutoFixture;
using NUnit.Framework;
using ShoppingCartWebApi.Models;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApiTests
{
    [SetUpFixture]
    public class TestSetup
    {
        protected IItem MockItem1;
        protected IItem MockItem2;

        [OneTimeSetUp]
        public void Init()
        {
            var fixture = new Fixture();

            MockItem1 = fixture.Build<Item>().With(x => x.Id, 100).With(x => x.Name, "Dummy Item 1")
                .With(x => x.Value, 100.00M).With(x => x.InventoryCount, 10).With(x => x.Description, "Dummy Item 1")
                .Create();

            MockItem2 = fixture.Build<Item>().With(x => x.Id, 200).With(x => x.Name, "Dummy Item 2")
                .With(x => x.Value, 50.00M).With(x => x.InventoryCount, 20).With(x => x.Description, "Dummy Item 2")
                .Create();
        }
    }
}