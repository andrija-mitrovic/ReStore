using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteProductCommandHandler> _logger;

        public DeleteProductCommandHandler(
            IApplicationDbContext context, 
            ILogger<DeleteProductCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Product with Id: {ProductId} not found.", request.Id);
                throw new NotFoundException(nameof(Product), request.Id);
            }

            _context.Products.Remove(product);

            await _context.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Product with Id: {ProductId} is successfully deleted.", product.Id);

            return Unit.Value;
        }
    }
}
