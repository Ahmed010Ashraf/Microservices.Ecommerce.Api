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
    public class GetAllBrandsQueryHandler : IRequestHandler<GetAllBrandsQuery, List<BrandResponseDto>>
    {
        private readonly IMapper _Mapper;
        private readonly IBrandReposatory _BrandReposatory;

        public GetAllBrandsQueryHandler(IMapper mapper ,IBrandReposatory BrandReposatory)
        {
            _Mapper = mapper;
            _BrandReposatory = BrandReposatory;
        }
        public async Task<List<BrandResponseDto>> Handle(GetAllBrandsQuery request, CancellationToken cancellationToken)
        {
            var brands = await _BrandReposatory.GetAllBrandsAsync();
            var brandDtos = _Mapper.Map<List<BrandResponseDto>>(brands);

            return brandDtos;
        }
    }
}
