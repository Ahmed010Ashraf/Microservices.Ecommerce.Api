using AutoMapper;
using Basket.Application.Commands;
using Basket.Core.Repository;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Application.Handlers.Commands
{
    public class DeleteBasketByUserNameCommandHandler : IRequestHandler<DeleteBasketByUserNameCommand, Unit>
    {
        private readonly IBasketRepository _BaskerRepo;

        public DeleteBasketByUserNameCommandHandler(IBasketRepository BaskerRepo, IMapper mapper)
        {
            _BaskerRepo = BaskerRepo;
        }
        public async Task<Unit> Handle(DeleteBasketByUserNameCommand request, CancellationToken cancellationToken)
        {
            await _BaskerRepo.DeleteBasket(request.UserName);

            return Unit.Value;
        }
    }
}
