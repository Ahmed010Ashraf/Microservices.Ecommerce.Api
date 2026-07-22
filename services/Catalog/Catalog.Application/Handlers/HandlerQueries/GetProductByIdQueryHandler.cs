using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Reposatories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.HandlerQueries
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProdcutResponseDto>
    {
        private readonly IMapper _Mapper;
        private readonly IProductReposatory _ProductRepo;

        public GetProductByIdQueryHandler(IMapper mapper , IProductReposatory ProductRepo)
        {
            _Mapper = mapper;
            _ProductRepo = ProductRepo;
        }
        public async Task<ProdcutResponseDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _ProductRepo.GetProductByIdAsync(request.Id);
            if (product == null)
            {
                throw new Exception("product not found");
            }

            var productResult = _Mapper.Map<ProdcutResponseDto>(product);

            return productResult;
        }
    }
}
