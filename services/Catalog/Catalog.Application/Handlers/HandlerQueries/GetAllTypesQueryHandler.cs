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
    public class GetAllTypesQueryHandler : IRequestHandler<GetAllTypesQuery, List<TypeResponseDto>>
    {
        private readonly IMapper _Mapper;
        private readonly ITypeReposatory _TypeRepo;

        public GetAllTypesQueryHandler(IMapper mapper , ITypeReposatory TypeRepo)
        {
            _Mapper = mapper;
            _TypeRepo = TypeRepo;
        }
        public async Task<List<TypeResponseDto>> Handle(GetAllTypesQuery request, CancellationToken cancellationToken)
        {
            var types = await _TypeRepo.GetAllTypesAsync();
            var typeDtos = _Mapper.Map<List<TypeResponseDto>>(types);
            return typeDtos;
        }
    }
}
