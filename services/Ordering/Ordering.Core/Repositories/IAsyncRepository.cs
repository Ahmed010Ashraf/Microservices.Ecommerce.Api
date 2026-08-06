using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Core.Repositories
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T,bool>>Predicit);

        Task<T?> GetById (int id);
        Task<T> AddAsync (T entity);

        Task UpdateAsync (T entity);

        Task DeleteAsync (T entity);
    }
}
