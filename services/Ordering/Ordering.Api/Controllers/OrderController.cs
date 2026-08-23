using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using Ordering.Infrastructure.Repositories;
using System.Net;

namespace Ordering.Api.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IMediator _Mediator;
        private readonly ILogger<OrderController> _Logger;

        public OrderController(IMediator mediator, ILogger<OrderController> logger)
        {
            _Mediator = mediator;
            _Logger = logger;
        }

        [HttpGet("{UserName}", Name = "GetOrdersByUserName")]
        [ProducesResponseType(typeof(List<OrderResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrderByUserName([FromRoute] string UserName)
        {
            var query = new GetOrderListQuery(UserName);
            var res = await _Mediator.Send(query);

            return Ok(res);
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckOutOrderCommand command)
        {
            var res = await _Mediator.Send(command);
            return Ok(res);
        }


        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            var res = await _Mediator.Send(command);
            return NoContent();
        }


        [HttpDelete("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            var res = await _Mediator.Send(command);
            return NoContent();
        }

    }
}
