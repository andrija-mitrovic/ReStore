using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateProductCommand> _logger;

        public CreateProductCommandHandler(
            IApplicationDbContext context, 
            IMapper mapper, 
            ILogger<CreateProductCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product {ProductId} is successfully created.", product.Id);

            return product.Id;
        }
    }
}
