using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
    {
        protected readonly OrderContext _Context;

        public RepositoryBase(OrderContext context)
        {
            _Context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> Predicit)
        {
            return await _Context.Set<T>().Where(Predicit).ToListAsync();
        }

        public async Task<T?> GetById(int id)
        {
            return await _Context.Set<T>().FindAsync(id);
        }

        public async Task<T> AddAsync(T entity)
        {
            await _Context.Set<T>().AddAsync(entity);
            await _Context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
             _Context.Entry(entity).State = EntityState.Modified;
            await _Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _Context.Set<T>().Remove(entity);
            await _Context.SaveChangesAsync();
        }

        

       
    }
}
