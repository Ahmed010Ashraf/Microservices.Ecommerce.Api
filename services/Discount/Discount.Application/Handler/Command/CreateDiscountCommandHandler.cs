using AutoMapper;
using Discount.Application.Command;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handler.Command
{
    public class CreateDiscountCommandHandler : IRequestHandler<CreateDiscountCommand, CouponModel>
    {
        private readonly IDiscountRepository _Repo;
        private readonly IMapper _Mapper;

        public CreateDiscountCommandHandler(IDiscountRepository repo , IMapper mapper)
        {
            _Repo = repo;
            _Mapper = mapper;
        }
        public async Task<CouponModel> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            var coupon = _Mapper.Map<Coupon>(request);
            var res = await _Repo.CreateDiscount(coupon);

            if (!res)
            {
                throw new Exception("can not create discount");
            }

            return _Mapper.Map<CouponModel>(coupon);
        }
    }
}
