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
    public static class TypeContextSeed
    {
        public static async Task SeedData(IMongoCollection<ProductType> TypeCollection)
        {
            if (await TypeCollection.Find(_ => true).AnyAsync())
            {
                return;
            }


            var filePath = Path.Combine("Data", "SeedData", "types.json");

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"there is no file here :{filePath}");
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var Types = JsonSerializer.Deserialize<List<ProductType>>(jsonData);
            if (Types != null && Types.Any())
            {
                await TypeCollection.InsertManyAsync(Types);
            }
        }
    }
}
