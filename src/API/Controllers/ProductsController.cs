using API.Extensions;
using Application.Common.DTOs;
using Application.Products.Commands.CreateProduct;
using Application.Products.Commands.DeleteProduct;
using Application.Products.Commands.UpdateProduct;
using Application.Products.Queries.GetProductById;
using Application.Products.Queries.GetProductsBrandsAndTypes;
using Application.Products.Queries.GetProductsWithPagination;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetProductsWithPagination([FromQuery] GetProductsWithPaginationQuery query)
        {
            var products = await Mediator.Send(query);
            Response.AddPaginationHeader(products.PageNumber, query.PageSize, products.TotalCount, products.TotalPages);

            return products.Items!;
        }

        //[HttpGet]
        //public async Task<ActionResult<List<ProductDto>>> GetProducts()
        //{
        //    return await Mediator.Send(new GetProductsQuery());
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            return await Mediator.Send(new GetProductByIdQuery(id));
        }

        [HttpPost]
        public async Task<ActionResult<int>> CreateProduct(CreateProductCommand command)
        {
            return await Mediator.Send(command);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, UpdateProductCommand command)
        {
            if (command.Id != id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await Mediator.Send(new DeleteProductCommand(id));

            return NoContent();
        }

        [HttpGet("filters")]
        public async Task<IActionResult> GetFilters()
        {
            return Ok(await Mediator.Send(new GetProductsBrandsAndTypesQuery()));
        }
    }
}
