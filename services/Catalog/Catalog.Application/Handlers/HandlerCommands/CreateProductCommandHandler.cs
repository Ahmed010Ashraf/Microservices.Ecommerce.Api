using AutoMapper;
using Catalog.Application.Commands;
using Catalog.Application.Responses;
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
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProdcutResponseDto>
    {
        private readonly IMapper _Mapper;
        private readonly IProductReposatory _ProductRepo;

        public CreateProductCommandHandler(IMapper mapper, IProductReposatory ProductRepo)
        {
            _Mapper = mapper;
            _ProductRepo = ProductRepo;
        }
        public async Task<ProdcutResponseDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _Mapper.Map<Product>(request);
            var createdProduct = await _ProductRepo.CreateProduct(product);

            var productResult = _Mapper.Map<ProdcutResponseDto>(createdProduct);

            return productResult;
        }
    }
}
