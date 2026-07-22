using Catalog.Application.Commands;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Specs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{

    public class CatalogController : BaseApiController
    {
        private readonly IMediator _Mediator;

        public CatalogController(IMediator Mediator)
        {
            _Mediator = Mediator;
        }
        [HttpGet("GetProdcutById/{id}")]
        [ProducesResponseType(typeof(ProdcutResponseDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ProdcutResponseDto>> GetProductById(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _Mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("GetAllProducts")]
        [ProducesResponseType(typeof(Pagination<ProdcutResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Pagination<ProdcutResponseDto>>> GetAllProducts([FromQuery] CatalogSpecsParams CatalogSpecsParams)
        {
            var query = new GetAllProductsQuery(CatalogSpecsParams);
            var result = await _Mediator.Send(query);
            return Ok(result);
        }


        [HttpGet("GetProductsByProductName/{ProductName}")]
        [ProducesResponseType(typeof(List<ProdcutResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<ProdcutResponseDto>>> GetProductsByProductName(string ProductName)
        {
            var query = new GetAllProductsByNameQuery(ProductName);
            var result = await _Mediator.Send(query);
            return Ok(result);
        }


        [HttpPost("CreateProduct")]
        [ProducesResponseType(typeof(ProdcutResponseDto), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<ProdcutResponseDto>> CreateProduct([FromBody] CreateProductCommand command)
        {
            var result = await _Mediator.Send(command);
            return Ok(result);

        }

        [HttpPut("UpdateProduct")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<bool>> UpdateProduct([FromBody] UpdateProductCommand command)
        {
            var result = await _Mediator.Send(command);
            return Ok(result);
        }


        [HttpDelete("DeleteProduct/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _Mediator.Send(command);
            return Ok(result);
        }



        [HttpGet("GetBrands")]
        [ProducesResponseType(typeof(List<BrandResponseDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IList<BrandResponseDto>>> GetBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _Mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetTypes")]
        [ProducesResponseType(typeof(List<TypeResponseDto>), (int)HttpStatusCode.OK)]

        public async Task<ActionResult<IList<TypeResponseDto>>> GetTypes()
        {
            var query = new GetAllTypesQuery();
            var result = await _Mediator.Send(query);
            return Ok(result);
        }

    }
}
