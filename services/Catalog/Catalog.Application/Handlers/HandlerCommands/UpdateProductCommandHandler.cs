using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Reposatories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.HandlerCommands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
    {
        private readonly IMapper _Mapper;
        private readonly IProductReposatory _ProductRepo;

        public UpdateProductCommandHandler(IMapper mapper, IProductReposatory ProductRepo)
        {
            _Mapper = mapper;
            _ProductRepo = ProductRepo;
        }
        public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _Mapper.Map<Product>(request);

            var result = await _ProductRepo.UpdateProduct(product);

            return result;
        }
    }
}
