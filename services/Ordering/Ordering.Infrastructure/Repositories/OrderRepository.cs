using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext context):base(context)
        {
            
        }
        public async Task<IEnumerable<Order>> GetOrderByUserName(string userName)
        {
            return await _Context.Orders.Where(o => o.UserName == userName).ToListAsync();
        }
    }
}
