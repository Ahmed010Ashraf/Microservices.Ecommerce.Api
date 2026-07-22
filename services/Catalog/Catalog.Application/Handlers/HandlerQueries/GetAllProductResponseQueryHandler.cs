using AutoMapper;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Reposatories;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Handlers.HandlerQueries
{
    public class GetAllProductResponseQueryHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProdcutResponseDto>>
    {
        private readonly IMapper _Mapper;
        private readonly IProductReposatory _ProductRepo;

        public GetAllProductResponseQueryHandler(IMapper mapper ,  IProductReposatory ProductRepo)
        {
            _Mapper = mapper;
            _ProductRepo = ProductRepo;
        }
        public async Task<Pagination<ProdcutResponseDto>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = await _ProductRepo.GetAllProductsAsync(request.Params);
            var productDtos = _Mapper.Map<Pagination<ProdcutResponseDto>>(products);
            return productDtos;
        }
    }
}
