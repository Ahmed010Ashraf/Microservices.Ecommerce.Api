using Discount.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Core.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string ProductName);

        Task<bool> CreateDiscount(Coupon Coupon);
        Task<bool> UpdateDiscount(Coupon Coupon);
        Task<bool> DeleteDiscount(string ProductName);
    }
}
