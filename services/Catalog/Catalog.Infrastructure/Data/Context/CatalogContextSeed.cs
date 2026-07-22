using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.Context
{
    public static class CatalogContextSeed
    {
        public static async Task SeedData(IMongoCollection<Product> ProductCollection)
        {
            if (await ProductCollection.Find(_ => true).AnyAsync())
            {
                return;
            }


            var filePath = Path.Combine("Data", "SeedData", "products.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"there is no file here :{filePath}");
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var products = JsonSerializer.Deserialize<List<Product>>(jsonData);
            if (products != null && products.Any())
            {
                await ProductCollection.InsertManyAsync(products);
            }
        }
    }
}

