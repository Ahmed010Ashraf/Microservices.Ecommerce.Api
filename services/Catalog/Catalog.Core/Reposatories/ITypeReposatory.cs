using Catalog.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Reposatories
{
    public interface ITypeReposatory
    {
        Task<IEnumerable<ProductType>> GetAllTypesAsync();
    }
}
