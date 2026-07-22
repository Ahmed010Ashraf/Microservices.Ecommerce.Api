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
    public static class BrandContextSeed
    {
        public static async Task SeedData(IMongoCollection<ProductBrand> BrandCollection)
        {
            if( await BrandCollection.Find(_=>true).AnyAsync())
            {
                return;
            }


            var filePath = Path.Combine("Data" , "SeedData", "brands.json");

            if(!File.Exists(filePath))
            {
                Console.WriteLine($"there is no file here :{filePath}");
                return;
            }

            var jsonData = await File.ReadAllTextAsync(filePath);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(jsonData);
            if (brands != null && brands.Any())
            {
                await BrandCollection.InsertManyAsync(brands);
            }
        }
    }
}
