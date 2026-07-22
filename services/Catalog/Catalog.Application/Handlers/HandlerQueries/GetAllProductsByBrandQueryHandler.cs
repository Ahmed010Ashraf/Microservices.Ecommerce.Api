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
    public class GetAllProductsByBrandQueryHandler : IRequestHandler<GetAllProductsByBrandQuery, IList<ProdcutResponseDto>>
    {

        private readonly IMapper _Mapper;
        private readonly IProductReposatory _ProductRepo;

        public GetAllProductsByBrandQueryHandler(IMapper mapper, IProductReposatory ProductRepo)
        {
            _Mapper = mapper;
            _ProductRepo = ProductRepo;
        }
        public async Task<IList<ProdcutResponseDto>> Handle(GetAllProductsByBrandQuery request, CancellationToken cancellationToken)
        {
            var products =await _ProductRepo.GetAllProductsByBrandAsync(request.BrandName);

            var productsResult = _Mapper.Map<IList<ProdcutResponseDto>>(products);

            return productsResult;
        }
    }
}
