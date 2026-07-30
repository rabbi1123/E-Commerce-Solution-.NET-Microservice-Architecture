using MediatR;
using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.Features.Cart.Requests.Commands
{
	public record UpdateCartCommand(CartHeaderDto Cart) : IRequest<Unit>;
}
