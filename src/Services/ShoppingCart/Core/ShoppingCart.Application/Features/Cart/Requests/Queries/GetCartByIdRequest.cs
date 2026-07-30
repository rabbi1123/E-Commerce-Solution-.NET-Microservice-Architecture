using MediatR;
using ShoppingCart.Application.DTOs.Cart;

namespace ShoppingCart.Application.Features.Cart.Requests.Queries
{
	public record GetCartByIdRequest(int Id) : IRequest<CartHeaderDto>;
}
