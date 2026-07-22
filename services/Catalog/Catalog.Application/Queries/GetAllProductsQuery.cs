using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllProductsQuery:IRequest<Pagination<ProdcutResponseDto>>
    {

        public CatalogSpecsParams Params { get; set; }
        public GetAllProductsQuery(CatalogSpecsParams _CatalogSpecsParams)
        {
            Params = _CatalogSpecsParams;
        }

    }
}
