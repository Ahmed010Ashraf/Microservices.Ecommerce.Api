using AutoMapper;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Mappers
{
    public class ShopingCartProfile:Profile
    {
        public ShopingCartProfile()
        {
            CreateMap<ShopingCart, ShopingCartResponse>().ReverseMap();
            CreateMap<ShopingCartItems, ShopingCartItemResponse>().ReverseMap();
            CreateMap<BasketCheckout , BasketCheckoutEvent>().ReverseMap();
            CreateMap<BasketCheckoutV2 , BasketCheckoutEventV2>().ReverseMap();

        }
    }
}
