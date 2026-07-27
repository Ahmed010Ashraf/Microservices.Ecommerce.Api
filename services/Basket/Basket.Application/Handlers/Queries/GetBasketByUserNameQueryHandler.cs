using AutoMapper;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Queries
{
    public class GetBasketByUserNameQueryHandler : IRequestHandler<GetBasketByUserNameQuery, ShopingCartResponse>
    {
        private readonly IBasketRepository _BaskerRepo;
        private readonly IMapper _Mapper;

        public GetBasketByUserNameQueryHandler(IBasketRepository BaskerRepo , IMapper mapper)
        {
            _BaskerRepo = BaskerRepo;
            _Mapper = mapper;
        }
        public async Task<ShopingCartResponse> Handle(GetBasketByUserNameQuery request, CancellationToken cancellationToken)
        {
            var basket = await _BaskerRepo.GetBasket(request.UserName);
            if (basket == null) {
                throw new Exception("basket not found ");
            }
            var result = _Mapper.Map<ShopingCartResponse>(basket);
            return result;
        }
    }
}
