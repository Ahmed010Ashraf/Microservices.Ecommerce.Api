using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Ordering.Application.commands;

namespace Ordering.Api.EventBusConsumer
{
    public class BasketOrderConsumer : IConsumer<BasketCheckoutEvent>
    {
        private readonly IMapper _Mapper;
        private readonly IMediator _Mediator;
        private readonly ILogger<BasketOrderConsumer> _Logger;

        public BasketOrderConsumer(IMapper mapper , IMediator mediator , ILogger<BasketOrderConsumer> logger)
        {
            _Mapper = mapper;
            _Mediator = mediator;
            _Logger = logger;
        }
        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            using var scope = _Logger.BeginScope("BasketOrderConsumer for {correlationid}" , context.Message.CorrelationId);
            var cmd = _Mapper.Map<CheckOutOrderCommand>(context.Message);
            var res = await _Mediator.Send(cmd);
            _Logger.LogInformation("Order {orderid} is successfully created for user {username}" , res , context.Message.UserName);


        }
    }
}
