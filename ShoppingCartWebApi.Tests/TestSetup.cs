using Moq;
using NUnit.Framework;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApiTests
{
    [SetUpFixture]
    public class TestSetup
    {
        [OneTimeSetUp]
        public void Init()
        {
            mockItem1 = new Mock<Item>(100, "Dummy Item 1", 100.00M, 10, "Dummy Item 1").Object;
            mockItem2 = new Mock<Item>(200, "Dummy Item 2", 50.00M, 20, "Dummy Item 2").Object;
        }
        
        protected Item mockItem1;
        protected Item mockItem2;
    }
}