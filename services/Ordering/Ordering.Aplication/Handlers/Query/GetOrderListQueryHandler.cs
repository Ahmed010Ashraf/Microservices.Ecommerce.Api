using AutoMapper;
using MediatR;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.Query
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrderResponse>>
    {
        private readonly IOrderRepository _Repo;
        private readonly IMapper _Mapper;

        public GetOrderListQueryHandler(IOrderRepository repo , IMapper mapper)
        {
            _Repo = repo;
            _Mapper = mapper;
        }
        public async Task<List<OrderResponse>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
           var orders = await  _Repo.GetOrderByUserName(request.UserName);
            if (orders == null || orders.Count() == 0)
            {
                throw new Exception("No orders found for the user");
            }
            var res = _Mapper.Map<List<OrderResponse>>(orders);
            return res;
        }
    }
}
