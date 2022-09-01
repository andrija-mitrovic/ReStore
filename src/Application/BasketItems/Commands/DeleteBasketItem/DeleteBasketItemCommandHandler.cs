using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.BasketItems.Commands.DeleteBasketItem
{
    public class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly ILogger<DeleteBasketItemCommandHandler> _logger;

        public DeleteBasketItemCommandHandler(
            IApplicationDbContext context, 
            ILogger<DeleteBasketItemCommandHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.BuyerId))
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Buyer Id is null or empty.", request.BuyerId);
                throw new Exception("Buyer Id is null or empty");
            }

            var basket = await _context.Baskets.Include(x => x.Items)
                                               .ThenInclude(x => x.Product)
                                               .FirstOrDefaultAsync(x => x.BuyerId == request.BuyerId);

            if (basket == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Basket with BuyerId: {BuyerId} was not found.", request.BuyerId);
                throw new NotFoundException(nameof(Basket), request.ProductId);
            }

            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Product with Id: {ProductId} was not found.", request.ProductId);
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            basket.RemoveItem(request.ProductId, request.Quantity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                var message = HelperFunction.GetMethodName() + " - Problem removing item from the basket.";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return Unit.Value;
        }
    }
}
