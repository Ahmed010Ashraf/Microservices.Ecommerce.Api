using Basket.Core.Entities;
using Basket.Core.Repository;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Basket.Infrastructure.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _Redis;

        public BasketRepository(IDistributedCache _redis)
        {
            _Redis = _redis;
        }

        public async Task<ShopingCart> GetBasket(string UserName)
        {
            var basket = await _Redis.GetStringAsync(UserName);
            if (basket == null)
            {
                return null;
            }
            var result = JsonSerializer.Deserialize<ShopingCart>(basket);
            return result;
        }

        public async Task<ShopingCart> UpdateBasket(ShopingCart shopingCart)
        {
            await _Redis.SetStringAsync(shopingCart.UserName, JsonSerializer.Serialize(shopingCart));

            return await GetBasket(shopingCart.UserName);
        }


        public async Task DeleteBasket(string UserName)
        {
            var basket = await _Redis.GetStringAsync(UserName);
            if (!string.IsNullOrEmpty(basket)) { 
                await _Redis.RemoveAsync(UserName);
            }
        }

      

    
    }
}
