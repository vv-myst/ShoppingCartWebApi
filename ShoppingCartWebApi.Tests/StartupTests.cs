﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using ShoppingCartWebApi;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;

namespace ShoppingCartWebApiTests
{
    [TestFixture]
    public class StartupTests
    {
        private IConfiguration mockConfig;
        private ServiceProvider serviceProvider;

        [SetUp]
        public void Initialize()
        {
            mockConfig = new Mock<IConfiguration>().Object;
            var startup = new Startup(mockConfig);
            var services = new ServiceCollection();
            startup.ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }
        
        [TestCase]
        public void ConfigureServices_AddsShoppingCartRepositoryService()
        {
            var shoppingCartRepo = serviceProvider.GetService<IShoppingCartRepository>();
            
            Assert.That(shoppingCartRepo, Is.Not.Null);
            Assert.That(shoppingCartRepo, Is.TypeOf<ShoppingCartRepository>());
        }
    }
}