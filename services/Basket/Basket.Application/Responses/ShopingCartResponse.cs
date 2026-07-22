using Basket.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Responses
{
    public class ShopingCartResponse
    {
        public string UserName { get; set; }

        public List<ShopingCartItems> Items { get; set; } = new List<ShopingCartItems>();

        public ShopingCartResponse()
        {

        }

        public ShopingCartResponse(string username)
        {
            UserName = username;
        }


        public decimal TotalPrice { get {
                decimal totalprice = 0;
                foreach (var item in Items)
                {
                    totalprice += (item.Price * item.Quantity); 
                }

                return totalprice;
            } }
    }
}
