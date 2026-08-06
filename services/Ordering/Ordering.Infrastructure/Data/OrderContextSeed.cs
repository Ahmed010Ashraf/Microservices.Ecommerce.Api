using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public async Task SeedAsync(OrderContext context, Logger<OrderContextSeed> logger)
        {
            if (!context.Orders.Any())
            {
                await context.Orders.AddRangeAsync(AddOrders());
                await context.SaveChangesAsync();
                logger.LogInformation("Seed data has been added to the database.");

            }
        }

        public IEnumerable<Core.Entities.Order> AddOrders()
        {
            var orders = new List<Core.Entities.Order>
            {
                new Core.Entities.Order
                {
                    UserName = "Ahmed",
                    FirstName = "Ahmed",
                    LastName = "Ali",
                    Email = "Ahmed@gmail.com",
                    AddressLine = "Cairo",
                    Country = "Egypt",
                    State = "Cairo",
                    PostalCode = "12345",
                    TotalPrice = 100,
                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    CreateDate = DateTime.Now,
                    CreatedBy = "Ahmed",
                    LastModifiedBy = "Ahmed",
                    LastModifiedDate = DateTime.Now
                }
            };
            return orders;

        }
    }
}
