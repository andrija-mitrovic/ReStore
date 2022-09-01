using Application.Common.DTOs;
using MediatR;

namespace Application.Products.Queries.GetProducts
{
    public class GetProductsQuery : IRequest<List<ProductDto>>
    {
    }
}
