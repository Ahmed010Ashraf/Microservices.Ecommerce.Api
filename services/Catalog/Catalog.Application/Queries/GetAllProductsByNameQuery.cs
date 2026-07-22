using Catalog.Application.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Queries
{
    public class GetAllProductsByNameQuery:IRequest<IList<ProdcutResponseDto>>
    {
        public string Name { get; set; }

        public GetAllProductsByNameQuery(string name)
        {
            Name = name;
        }
    }
}
