using MediatR;

namespace Application.BasketItems.Commands.DeleteBasketItem
{
    public class DeleteBasketItemCommand : IRequest
    {
        public string? BuyerId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
