using MediatR;
using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.Features.Cart.Requests.Commands
{
	public record CheckoutCart(BasketCheckout BasketCheckout) : IRequest<Unit>;
}
