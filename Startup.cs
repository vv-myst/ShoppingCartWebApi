﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingCartWebApi.InMemoryRepository;
using ShoppingCartWebApi.InMemoryRepository.Interfaces;
using ShoppingCartWebApi.Models.Handlers;
using ShoppingCartWebApi.Models.Interfaces;

namespace ShoppingCartWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var shoppingCartRepository = new ShoppingCartRepository();
            var shoppingCartHandler = new ShoppingCartHandler(shoppingCartRepository);
            
            services.AddSingleton<IShoppingCartRepository>(item => shoppingCartRepository);
            services.AddScoped<IShoppingCartHandler>(item => shoppingCartHandler);
            services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseMvc();
        }
    }
}