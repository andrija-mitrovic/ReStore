using Application.Common.DTOs;
using MediatR;

namespace Application.BasketItems.Commands.AddBasketItem
{
    public class AddBasketItemCommand : IRequest<BasketDto>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? BuyerId { get; set; }
    }
}
