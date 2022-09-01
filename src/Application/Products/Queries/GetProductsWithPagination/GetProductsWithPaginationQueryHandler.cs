using Application.Common.DTOs;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Products.Queries.GetProductsWithPagination
{
    public class GetProductsWithPaginationQueryHandler : IRequestHandler<GetProductsWithPaginationQuery, PaginatedList<ProductDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetProductsWithPaginationQueryHandler(
            IApplicationDbContext context, 
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ProductDto>> Handle(GetProductsWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Products
                                 .Sort(request.OrderBy)
                                 .Search(request.SearchTerm)
                                 .Filter(request.Brands, request.Types)
                                 .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                                 .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
