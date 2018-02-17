#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi EmptyShoppingCartController.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-15 11:41 
// 2018-02-15 11:41 

#endregion

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Contracts;

namespace ShoppingCartWebApi.Controllers
{
    [Route("api/shoppingcart")]
    public class EmptyShoppingCartController : IEmptyShoppingCartController
    {
        public EmptyShoppingCartController(ShoppingCartEntities shoppingCartEntities)
        {
            this.shoppingCartEntities = shoppingCartEntities;
        }
        
        private readonly ShoppingCartEntities shoppingCartEntities;
        
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await Task.Run(() =>
            {
                RemoveAllItemFromShoppingCart();
                UpdateItemCountInShoppingCart();
                UpdateShoppingCartValues();
            });
            
            return new ObjectResult(shoppingCartEntities.InMemoryShoppingCart)
            {
                StatusCode = (int) HttpStatusCode.OK
            };
        }
        
        private void RemoveAllItemFromShoppingCart()
        {
            shoppingCartEntities.InMemoryShoppingCart.ItemList.Clear();   
        }

        private void UpdateItemCountInShoppingCart()
        {
            shoppingCartEntities.InMemoryShoppingCart.ClearItemCountMap();
        }

        private void UpdateShoppingCartValues()
        {
            shoppingCartEntities.InMemoryShoppingCart.UpdateShoppingCart();
        }
    }
}
