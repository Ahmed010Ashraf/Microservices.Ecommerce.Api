using Catalog.Core.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data.Context
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> products { get; }
        IMongoCollection<ProductBrand> Brands { get; }
        IMongoCollection<ProductType> Types { get; }
    }
}
