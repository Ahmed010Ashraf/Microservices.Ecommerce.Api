using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Commands
{
    public class CreateShopingCartCommand:IRequest<ShopingCartResponse>
    {
        public string UserName { get; set; }
        public List<ShopingCartItems> Items { get; set; }

        public CreateShopingCartCommand(string username , List<ShopingCartItems> items)
        {
            UserName = username;
            Items = items;
        }
    }
}
