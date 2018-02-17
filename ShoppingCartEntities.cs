#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi ShoppingCartEntities.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-15 11:40 
// 2018-02-15 11:19 

#endregion

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi
{
    public class ShoppingCartEntities
    {
        public ShoppingCartEntities()
        {
            InMemoryShoppingCart = new ShoppingCart(new List<Item>());    
        }
        
        public ShoppingCart InMemoryShoppingCart { get; }
    }
}
