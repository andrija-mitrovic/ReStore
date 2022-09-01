using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            IApplicationDbContext context, 
            IMapper mapper, 
            ILogger<UpdateProductCommandHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Product with Id: {ProductId} was not found.", request.Id);
                throw new NotFoundException(nameof(Product), request.Id);
            }

            _mapper.Map(request, product);

            _context.Products.Update(product!);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
