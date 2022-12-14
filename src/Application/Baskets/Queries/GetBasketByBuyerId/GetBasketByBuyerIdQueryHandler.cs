using Application.Common.DTOs;
using Application.Common.Exceptions;
using Application.Common.Helpers;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Baskets.Queries.GetBasketByBuyerId
{
    public class GetBasketByBuyerIdQueryHandler : IRequestHandler<GetBasketByBuyerIdQuery, BasketDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetBasketByBuyerIdQuery> _logger;

        public GetBasketByBuyerIdQueryHandler(IApplicationDbContext context, IMapper mapper, ILogger<GetBasketByBuyerIdQuery> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BasketDto> Handle(GetBasketByBuyerIdQuery request, CancellationToken cancellationToken)
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
                throw new NotFoundException(nameof(Basket), request.BuyerId!);
            }

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
