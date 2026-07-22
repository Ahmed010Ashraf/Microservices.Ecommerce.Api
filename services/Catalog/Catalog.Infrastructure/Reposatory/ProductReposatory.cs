using Catalog.Core.Entities;
using Catalog.Core.Reposatories;
using Catalog.Core.Specs;
using Catalog.Infrastructure.Data.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Reposatory
{
    public class ProductReposatory(ICatalogContext _context) : IProductReposatory, IBrandReposatory, ITypeReposatory
    {

        public async Task<IEnumerable<ProductBrand>> GetAllBrandsAsync()
        {
            return await _context.Brands.Find(_=>true).ToListAsync();
        }

        public async Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecsParams CatalogSpecsParams)
        {
            var filter = Builders<Product>.Filter.Empty;

            if(!string.IsNullOrEmpty(CatalogSpecsParams.BrnadId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Brand.Id, CatalogSpecsParams.BrnadId);
            }

            if(!string.IsNullOrEmpty(CatalogSpecsParams.TypeId))
            {
                filter &= Builders<Product>.Filter.Eq(p => p.Type.Id, CatalogSpecsParams.TypeId);
            }

            if(!string.IsNullOrEmpty(CatalogSpecsParams.search))
            {
                filter &= Builders<Product>.Filter.Where(p=>p.Name.ToLower().Contains(CatalogSpecsParams.search.ToLower()));
            }

            var sort = Builders<Product>.Sort.Ascending(p => p.Name);

            switch (CatalogSpecsParams.Sort)
            {
                case "priceAsc":
                    sort = Builders<Product>.Sort.Ascending(p=>p.Price); break;
                case "priceDsc":
                    sort = Builders<Product>.Sort.Descending(p=>p.Price); break;
                default:
                    break;
            }
            var count = await _context.products.CountDocumentsAsync(filter);
            var data = await _context.products.Find(filter).Sort(sort).Skip(CatalogSpecsParams.PageSize * (CatalogSpecsParams.PageIndex - 1)).Limit(CatalogSpecsParams.PageSize).ToListAsync();

            return new Pagination<Product> { 
                PageSize = CatalogSpecsParams.PageSize,
                PageIndex = CatalogSpecsParams.PageIndex,
                Count =(int) count,
                Data = data
            };
        }

        public async Task<IEnumerable<Product>> GetAllProductsByBrandAsync(string name)
        {
            return await _context.products.Find(p => p.Brand.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsByNameAsync(string name)
        {
            return await _context.products.Find(p => p.Name == name).ToListAsync();
        }

        public async Task<IEnumerable<ProductType>> GetAllTypesAsync()
        {
            return await _context.Types.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Product> CreateProduct(Product product)
        {
             await _context.products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var result = await _context.products.ReplaceOneAsync(p => p.Id == product.Id, product);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            var result = await _context.products.DeleteOneAsync(p => p.Id == id);

            return result.IsAcknowledged && result.DeletedCount > 0;
        }

      
      
    }
}
