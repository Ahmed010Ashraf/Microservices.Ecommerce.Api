using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.commands;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.command
{
    public class CheckOutOrderCommandHandler : IRequestHandler<CheckOutOrderCommand, int>
    {
        private readonly ILogger<CheckOutOrderCommandHandler> _Logger;
        private readonly IMapper _Mapper;
        private readonly IOrderRepository _Repo;

        public CheckOutOrderCommandHandler(ILogger<CheckOutOrderCommandHandler> logger , IMapper mapper , IOrderRepository repo)
        {
            _Logger = logger;
            _Mapper = mapper;
            _Repo = repo;
        }
        public async Task<int> Handle(CheckOutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _Mapper.Map<Order>(request);
            var res = await _Repo.AddAsync(orderEntity);
            _Logger.LogInformation($"Order with id : {res.Id} is successfully created");
            return res.Id;
        }
    }
}
