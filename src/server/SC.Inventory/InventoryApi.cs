using System.ComponentModel;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SC.Inventory.Models;
using SC.Inventory.Services;

namespace SC.Inventory;

public static class InventoryApi
{
    public static IEndpointRouteBuilder MapInventoryApi(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api/inventory");

        api.MapGet("/list/{id:int}", GetItemByID)
            .WithName("GetItem")
            .WithSummary("Get inventory item")
            .WithDescription("Get an item from the inventory")
            .WithTags("Items");

        return app;
    }

    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest, "application/problem+json")]
    public static async Task<Results<Ok<InventoryItem>, NotFound, BadRequest<ProblemDetails>>> GetItemByID(
        HttpContext httpContext,
        [AsParameters] InventoryService service,
        [Description("The catalog item id")] int id)
    {
        if (id <= 0)
        {
            return TypedResults.BadRequest<ProblemDetails>(new()
            {
                Detail = "Id is not valid"
            });
        }

        var item = await service.Context.InventoryItems.SingleOrDefaultAsync(ci => ci.Id == id);

        if (item == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(item);
    }
}
