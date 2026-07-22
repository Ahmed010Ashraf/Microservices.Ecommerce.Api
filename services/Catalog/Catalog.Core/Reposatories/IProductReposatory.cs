using Catalog.Core.Entities;
using Catalog.Core.Specs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Reposatories
{
    public interface IProductReposatory
    {
        Task<Pagination<Product>> GetAllProductsAsync(CatalogSpecsParams CatalogSpecsParams);

        Task<Product> GetProductByIdAsync(string id);

        Task<IEnumerable<Product>> GetAllProductsByBrandAsync(string name);
        Task<IEnumerable<Product>> GetAllProductsByNameAsync(string name);

        Task<Product> CreateProduct(Product product);

        Task<bool> UpdateProduct(Product product);

        Task<bool> DeleteProduct(string id);


    }
}
