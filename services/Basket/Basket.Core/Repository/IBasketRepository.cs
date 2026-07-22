using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Repository
{
    public interface IBasketRepository
    {
        Task<ShopingCart> GetBasket(string UserName);
        Task<ShopingCart> UpdateBasket (ShopingCart shopingCart);

        Task DeleteBasket (string UserName);
    }
}
