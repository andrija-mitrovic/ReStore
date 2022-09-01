using FluentValidation;

namespace Application.BasketItems.Commands.AddBasketItem
{
    public class AddBasketItemCommandValidator : AbstractValidator<AddBasketItemCommand>
    {
        public AddBasketItemCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Quantity).NotEmpty();
        }
    }
}
