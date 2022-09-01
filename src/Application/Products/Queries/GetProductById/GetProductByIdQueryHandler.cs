using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;

        public GetProductByIdQueryHandler(
            IApplicationDbContext context, 
            IMapper mapper, 
            ILogger<GetProductByIdQueryHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Product with Id: {ProductId} was not found.", request.Id);
                throw new NotFoundException(nameof(Product), request.Id);
            }

            return _mapper.Map<ProductDto>(product);
        }
    }
}
