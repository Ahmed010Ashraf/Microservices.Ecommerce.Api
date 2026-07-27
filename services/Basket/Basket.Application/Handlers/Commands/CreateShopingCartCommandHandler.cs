using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Responses;
using Basket.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Commands
{
    public class CreateShopingCartCommandHandler : IRequestHandler<CreateShopingCartCommand, ShopingCartResponse>
    {

        private readonly IBasketRepository _BaskerRepo;
        private readonly IMapper _Mapper;

        public CreateShopingCartCommandHandler(IBasketRepository BaskerRepo, IMapper mapper)
        {
            _BaskerRepo = BaskerRepo;
            _Mapper = mapper;
        }
        public async Task<ShopingCartResponse> Handle(CreateShopingCartCommand request, CancellationToken cancellationToken)
        {

            //here we will integrate with discount service in the future
            var basket = await _BaskerRepo.UpdateBasket(new Core.Entities.ShopingCart()
            {
                UserName = request.UserName,
                Items = request.Items,

            });

            var res = _Mapper.Map<ShopingCartResponse>(basket);

            return res;
        }
    }
}
