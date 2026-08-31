using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.commands;

namespace Ordering.Api.EventBusConsumer
{
    public class BasketOrderConsumerV2 : IConsumer<BasketCheckoutEventV2>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly ILogger<BasketOrderConsumerV2> _Logger;

        public BasketOrderConsumerV2(IMapper mapper, IMediator mediator, ILogger<BasketOrderConsumerV2> logger)
        {
            _Mapper = mapper;
            _Mediator = mediator;
            _Logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEventV2> context)
        {
            using var scope = _Logger.BeginScope("BasketOrderConsumer for {correlationid} from api v2", context.Message.CorrelationId);
            var cmd = _Mapper.Map<CheckOutOrderCommandV2>(context.Message);
            var res = await _Mediator.Send(cmd);
            _Logger.LogInformation("Order {orderid} is successfully created for user {username} from api v2", res, context.Message.UserName);


        }
    }
}

