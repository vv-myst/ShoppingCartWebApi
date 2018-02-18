using System.Net;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartWebApi.Models;

namespace ShoppingCartWebApi.Controllers.HttpErrors
{
    public static class HttpErrorResults
    {
        public static IActionResult ErrorItemNotFound(this int itemId)
        {
            var error = new ObjectResult($"No Item found for the given ItemId: {itemId}")
            {
                StatusCode = (int) HttpStatusCode.BadRequest
            };
            return error;
        }

        public static IActionResult ErrorItemIdsDoNotMatch(this Item item, int itemId)
        {
            var error = new ObjectResult(
                $"ItemId in the Url: {itemId} and ItemId of the Item in the body: {item.Id} do not match")
            {
                StatusCode = (int) HttpStatusCode.BadRequest
            };
            return error;
        }
    }
}