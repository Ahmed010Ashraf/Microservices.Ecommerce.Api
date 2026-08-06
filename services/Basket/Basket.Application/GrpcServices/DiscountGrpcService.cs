using Discount.Grpc.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.GrpcServices
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtoService.DiscountProtoServiceClient _Discountservice;

        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountservice)
        {
            _Discountservice = discountservice;
        }

        public async Task<CouponModel>? GetDiscount(string productName)
        {
            var request = new GetDiscountRequest { ProductName = productName };
            return await _Discountservice.GetDiscountAsync(request);
        }
    }
}
