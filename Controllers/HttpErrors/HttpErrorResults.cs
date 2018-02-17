using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartWebApi.Controllers.HttpErrors
{
    public static class HttpErrorResults
    {
        public static IActionResult ErrorNoItemFound(this int itemId)
        {
            var error = new ObjectResult($"No Item found for the given ItemId: {itemId}")
            {
                StatusCode = (int) HttpStatusCode.BadRequest
            };
            return error;
        }
    }
}