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
    public class CheckOutOrderCommandV2Handler : IRequestHandler<CheckOutOrderCommandV2, int>
    {
        private readonly ILogger<CheckOutOrderCommandV2Handler> _Logger;
        private readonly IMapper _Mapper;
        private readonly IOrderRepository _Repo;

        public CheckOutOrderCommandV2Handler(ILogger<CheckOutOrderCommandV2Handler> logger, IMapper mapper, IOrderRepository repo)
        {
            _Logger = logger;
            _Mapper = mapper;
            _Repo = repo;
        }
        public async Task<int> Handle(CheckOutOrderCommandV2 request, CancellationToken cancellationToken)
        {
            var orderEntity = _Mapper.Map<Order>(request);
            var res = await _Repo.AddAsync(orderEntity);
            _Logger.LogInformation($"Order with id : {res.Id} is successfully created from api v2");
            return res.Id;
        }
    }
}
