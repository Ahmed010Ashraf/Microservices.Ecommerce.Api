using Discount.Application.Command;
using Discount.Application.Query;
using Discount.Grpc.Protos;
using Grpc.Core;
using MediatR;

namespace Discount.Api.Services
{
    public class DiscountService:DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IMediator _Mediator;

        public DiscountService(IMediator mediator)
        {
            _Mediator = mediator;
        }

        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var query = new GetDiscountQuery(request.ProductName);
            var res = await _Mediator.Send(query);
            return res;
        }


        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var command = new CreateDiscountCommand()
            {
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                ProductName = request.Coupon.ProductName,
            };
            return await _Mediator.Send(command);
        }


        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var command = new UpdateDiscountCommand()
            {
                Id = request.Coupon.Id,
                Amount = request.Coupon.Amount,
                Description = request.Coupon.Description,
                ProductName = request.Coupon.ProductName,
            };

            var res = await _Mediator.Send(command);

            return res;
        }


        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var command = new DeleteDiscountCommand(request.ProductName);

            var res = await _Mediator.Send(command);

            return new DeleteDiscountResponse()
            {
                Success = res
            };
        }
    }
}
