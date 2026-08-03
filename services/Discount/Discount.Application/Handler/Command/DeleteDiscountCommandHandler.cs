using AutoMapper;
using Discount.Application.Command;
using Discount.Core.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.Application.Handler.Command
{
    public class DeleteDiscountCommandHandler : IRequestHandler<DeleteDiscountCommand, bool>
    {
        private readonly IDiscountRepository _Repo;
        private readonly IMapper _Mapper;

        public DeleteDiscountCommandHandler(IDiscountRepository repo, IMapper mapper)
        {
            _Repo = repo;
            _Mapper = mapper;
        }
        public async Task<bool> Handle(DeleteDiscountCommand request, CancellationToken cancellationToken)
        {
            var res = await _Repo.DeleteDiscount(request.ProductName);

            if (!res)
            {
                throw new Exception("can not delete the discount");
            }
            return res; 
        }
    }
}
