using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContext(DbContextOptions<OrderContext>opt):DbContext(opt)
    {

        public DbSet<Order> Orders { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach(var entity in ChangeTracker.Entries<EntityBase>())
            {
                switch(entity.State)
                {
                    case EntityState.Added:
                        entity.Entity.CreateDate = DateTime.UtcNow;
                        entity.Entity.CreatedBy = "Ahmed"; // to be added by auth server
                        break;
                    case EntityState.Modified:
                        entity.Entity.LastModifiedDate = DateTime.UtcNow;
                        entity.Entity.LastModifiedBy = "Ahmed";// to be added by auth server
                        break;
                }

                
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess , cancellationToken);
        }
    }
}
