using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers.V2
{
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {

        private readonly IMediator _Mediator;
        private readonly IPublishEndpoint _Publishendpoint;
        private readonly IMapper _Mapper;
        private readonly ILogger<BasketController> _Logger;

        public BasketController(IMediator mediator,
            IPublishEndpoint publishendpoint,
            IMapper mapper,
            ILogger<BasketController> logger)
        {
            _Mediator = mediator;
            _Publishendpoint = publishendpoint;
            _Mapper = mapper;
            _Logger = logger;
        }



        [Route("checkout")]
        [HttpPost]

        public async Task<IActionResult> checkout([FromBody] BasketCheckoutV2 basketcheckout)
        {
            //ensure that basket exists for the user before proceeding to checkout
            var query = new GetBasketByUserNameQuery(basketcheckout.UserName);
            var basket = await _Mediator.Send(query);
            if (basket == null)
            {
                return BadRequest();
            }

            //createing and send the basket checkout event to the message broker
            var eventmessage = _Mapper.Map<BasketCheckoutEventV2>(basketcheckout);
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
