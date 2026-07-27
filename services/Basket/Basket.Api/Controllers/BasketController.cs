using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly IMediator _Mediator;

        public BasketController(IMediator mediator )
        {
            _Mediator = mediator;
        }

        [HttpGet("GetBasketByUserName/{UserName}")]
        public async Task<ActionResult<ShopingCartResponse>> GetBasket(string UserName)
        {
            var query = new GetBasketByUserNameQuery(UserName);
            var res = await _Mediator.Send(query);

            return Ok(res);
        }


        [HttpPost("CreateOrUpdateBasket")]
        public async Task<ActionResult<ShopingCartResponse>> CreateOrUpdateBasket([FromBody] CreateShopingCartCommand command)
        {
            var res = await _Mediator.Send(command);
            return Ok(res);
        }

        [HttpDelete("DeleteBasket")]
        public async Task<Unit> DeleteBasket(string username)
        {
            var command = new DeleteBasketByUserNameCommand(username);
            var res = await _Mediator.Send(command);
            return res;
            
        }
    }
}
