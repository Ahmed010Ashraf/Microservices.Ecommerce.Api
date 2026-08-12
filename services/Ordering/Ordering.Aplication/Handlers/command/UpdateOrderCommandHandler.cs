using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.commands;
using Ordering.Application.Exceptions;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Handlers.command
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
    {
        private readonly ILogger<UpdateOrderCommandHandler> _Logger;
        private readonly IMapper _Mapper;
        private readonly IOrderRepository _Repo;

        public UpdateOrderCommandHandler(ILogger<UpdateOrderCommandHandler> logger, IMapper mapper, IOrderRepository repo)
        {
            _Logger = logger;
            _Mapper = mapper;
            _Repo = repo;
        }
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var isexist = await _Repo.GetById(request.Id);
            if (isexist is null)
            {
                throw new OrderNotFoundException(nameof(Order), request.Id);
            }
            var orderEntity = _Mapper.Map<Order>(request);
            await _Repo.UpdateAsync(orderEntity);

            return Unit.Value;
        }
    }
}
