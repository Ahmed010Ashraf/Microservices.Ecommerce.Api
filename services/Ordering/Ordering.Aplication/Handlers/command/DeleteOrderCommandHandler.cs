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
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
    {
        private readonly ILogger<DeleteOrderCommandHandler> _Logger;
        private readonly IOrderRepository _Repo;

        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger , IOrderRepository repo)
        {
            _Logger = logger;
            _Repo = repo;
        }
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _Repo.GetById(request.Id);
            if(order is null)
            {
                throw new OrderNotFoundException(nameof(Order) , request.Id );
            }

            await _Repo.DeleteAsync(order);
            _Logger.LogInformation($"order with id {request.Id} is deleted");

            return Unit.Value;

        }
    }
}
