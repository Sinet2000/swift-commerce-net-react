
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using SC.Inventory.Models;

namespace SC.Inventory.Infrastructure;

public class InventoryContextSeed(
    IWebHostEnvironment env,
    ILogger<InventoryContextSeed> logger) : IDbSeeder<InventoryContext>
{
    public async Task SeedAsync(InventoryContext context)
    {
        var contentRootPath = env.ContentRootPath;

        context.Database.OpenConnection();
        ((NpgsqlConnection)context.Database.GetDbConnection()).ReloadTypes();

        if (!context.InventoryItems.Any())
        {
            var sourcePath = Path.Combine(contentRootPath, "Setup", "inventory.json");
            var sourceJson = File.ReadAllText(sourcePath);
            var sourceItems = JsonSerializer.Deserialize<InventorySourceEntry[]>(sourceJson) ?? [];

            // context.CatalogBrands.RemoveRange(context.CatalogBrands);
            // await context.CatalogBrands.AddRangeAsync(sourceItems.Select(x => x.Brand).Distinct()
            //     .Where(brandName => brandName != null)
            //     .Select(brandName => new CatalogBrand(brandName!)));
            // logger.LogInformation("Seeded catalog with {NumBrands} brands", context.CatalogBrands.Count());

            // context.CatalogTypes.RemoveRange(context.CatalogTypes);
            // await context.CatalogTypes.AddRangeAsync(sourceItems.Select(x => x.Type).Distinct()
            //     .Where(typeName => typeName != null)
            //     .Select(typeName => new CatalogType(typeName!)));
            // logger.LogInformation("Seeded catalog with {NumTypes} types", context.CatalogTypes.Count());

            // await context.SaveChangesAsync();

            // var brandIdsByName = await context.CatalogBrands.ToDictionaryAsync(x => x.Brand, x => x.Id);
            // var typeIdsByName = await context.CatalogTypes.ToDictionaryAsync(x => x.Type, x => x.Id);

            var inventoryItems = sourceItems
                .Where(source => source.Name != null && source.Brand != null && source.Type != null)
                .Select(source => new InventoryItem(source.Name!)
                {
                    Id = source.Id,
                    Description = source.Description
                }).ToArray();

            await context.InventoryItems.AddRangeAsync(inventoryItems);
            logger.LogInformation("Seeded inventory with {NumItems} items", context.InventoryItems.Count());
            await context.SaveChangesAsync();
        }
    }

    private class InventorySourceEntry
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
    }
}
