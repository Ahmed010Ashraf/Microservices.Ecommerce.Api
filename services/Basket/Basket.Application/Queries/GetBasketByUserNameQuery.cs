using Basket.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Queries
{
    public class GetBasketByUserNameQuery:IRequest<ShopingCartResponse>
    {
        public string UserName { get; set; }
        public GetBasketByUserNameQuery(string username)
        {
            UserName = username;
        }
    }
}
