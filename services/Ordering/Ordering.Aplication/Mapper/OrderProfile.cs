using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.commands;
using Ordering.Application.Responses;
using Ordering.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Mapper
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<Order,OrderResponse>().ReverseMap();
            CreateMap<CheckOutOrderCommand,Order>().ReverseMap();
            CreateMap<UpdateOrderCommand,Order>().ReverseMap();
            CreateMap<CheckOutOrderCommand, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
