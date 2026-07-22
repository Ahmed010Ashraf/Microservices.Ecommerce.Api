using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.Context
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> products { get; }

        public IMongoCollection<ProductBrand> Brands { get; }

        public IMongoCollection<ProductType> Types { get; }

        public CatalogContext(IConfiguration configrations)
        {
            var client = new MongoClient(configrations["DataBaseSettings:ConnectionString"]);
            var database = client.GetDatabase(configrations["DataBaseSettings:DatabaseName"]);


            products = database.GetCollection<Product>(configrations["DataBaseSettings:ProductCollection"]);
            Brands = database.GetCollection<ProductBrand>(configrations["DataBaseSettings:BrandCollection"]);
            Types = database.GetCollection<ProductType>(configrations["DataBaseSettings:TypeCollection"]);


            _=BrandContextSeed.SeedData(Brands);
            _=TypeContextSeed.SeedData(Types);
            _=CatalogContextSeed.SeedData(products);



        }
    }
}
