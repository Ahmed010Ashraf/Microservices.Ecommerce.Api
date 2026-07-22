using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Core.Entities
{
    public class ShopingCart
    {
        public string UserName { get; set; }

        public List<ShopingCartItems> Items { get; set; }= new List<ShopingCartItems>();

        public ShopingCart()
        {
            
        }

        public ShopingCart(string username)
        {
            UserName = username;
        }
    }
}
