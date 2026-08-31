using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers
{
    [ApiVersion("1")]
    public class BasketController : BaseApiController
    {
        private readonly IMediator _Mediator;
        private readonly IPublishEndpoint _Publishendpoint;
        private readonly IMapper _Mapper;
        private readonly ILogger<BasketController> _Logger;

        public BasketController(IMediator mediator ,
            IPublishEndpoint publishendpoint ,
            IMapper mapper , 
            ILogger<BasketController>logger)
        {
            _Mediator = mediator;
            _Publishendpoint = publishendpoint;
            _Mapper = mapper;
            _Logger = logger;
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

        [Route("checkout")]
        [HttpPost]
        
        public async Task<IActionResult> checkout([FromBody] BasketCheckout basketcheckout)
        {
            //ensure that basket exists for the user before proceeding to checkout
            var basket = await _Mediator.Send(new GetBasketByUserNameQuery(basketcheckout.UserName));
            if(basket == null)
            {
                return BadRequest();
            }

            //createing and send the basket checkout event to the message broker
            var eventmessage = _Mapper.Map<BasketCheckoutEvent>(basketcheckout);
            eventmessage.TotalPrice = basket.TotalPrice;
            await _Publishendpoint.Publish(eventmessage);

            _Logger.LogInformation($"BasketCheckoutEvent is published with UserName : {basketcheckout.UserName} and TotalPrice : {basket.TotalPrice}");

            //delete the basket after checkout
            var command = new DeleteBasketByUserNameCommand(basketcheckout.UserName);
            await _Mediator.Send(command);

            return Accepted();
        }
    }
}
