using Application.Common.Constants;
using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.BasketItems.Commands.AddBasketItem
{
    internal class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, BasketDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICookieService _cookieService;
        private readonly IMapper _mapper;
        private readonly ILogger<AddBasketItemCommand> _logger;

        public AddBasketItemCommandHandler(
            IApplicationDbContext context, 
            ICookieService cookieService, 
            IMapper mapper, 
            ILogger<AddBasketItemCommand> logger)
        {
            _context = context;
            _cookieService = cookieService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BasketDto> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
        {
            var basket = await _context.Baskets.Include(x => x.Items)
                                               .ThenInclude(x => x.Product)
                                               .FirstOrDefaultAsync(x => x.BuyerId == request.BuyerId);

            if (basket == null)
            {
                basket = await CreateBasket();
            }

            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);

            if (product == null)
            {
                _logger.LogError(HelperFunction.GetMethodName() + " - Product with Id: {ProductId} was not found.", request.ProductId);
                throw new NotFoundException(nameof(Product), request.ProductId);
            }

            basket.AddItem(product, request.Quantity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                var message = HelperFunction.GetMethodName() + " - Problem saving item to basket.";
                _logger.LogError(message);
                throw new Exception(message);
            }

            return _mapper.Map<BasketDto>(basket);
        }

        private async Task<Basket> CreateBasket()
        {
            var buyerId = Guid.NewGuid().ToString();

            _cookieService.AddCookie(CookieConstants.KEY, buyerId);

            var basket = new Basket { BuyerId = buyerId };

            await _context.Baskets.AddAsync(basket!);

            return basket;
        }
    }
}
