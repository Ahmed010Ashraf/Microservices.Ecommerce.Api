using Discount.Application.Query;
using Discount.Core.Entities;
using Discount.Core.Repositories;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handler.Queries
{
    public class GetDiscountQueryHandler : IRequestHandler<GetDiscountQuery, CouponModel>
    {
        private readonly ILogger<GetDiscountQueryHandler> _Logger;
        private readonly IDiscountRepository _Repo;

        public GetDiscountQueryHandler(ILogger<GetDiscountQueryHandler> Logger , IDiscountRepository Repo)
        {
            _Logger = Logger;
            _Repo = Repo;
        }
        public async Task<CouponModel> Handle(GetDiscountQuery request, CancellationToken cancellationToken)
        {
            var coupon = await _Repo.GetDiscount(request.ProductName);

            if (coupon == null)
            {
                throw new RpcException(new Status( StatusCode.NotFound ,$"there is no discount for this product : {request.ProductName}" ));
            }

            var result = new CouponModel()
            {
                Id = coupon.Id,
                ProductName = coupon.ProductName,
                Description = coupon.Description,
                Amount = coupon.Amount,
            };

            return result;
        }
    }
}
