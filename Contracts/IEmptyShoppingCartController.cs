#region Auto generated information. Please do not modify

// ShoppingCartWebApi ShoppingCartWebApi IEmptyShoppingCartController.cs
// bila007 Bilangi, Vivek-Vardhan
// 2018-02-15 11:15 
// 2018-02-15 11:14 

#endregion

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartWebApi.Contracts
{
    public interface IEmptyShoppingCartController
    {
        /// <summary>
        ///     Delete all the items in the shopping cart
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> Delete();
    }
}
