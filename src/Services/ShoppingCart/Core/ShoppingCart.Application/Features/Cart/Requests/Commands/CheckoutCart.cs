using MediatR;
using ShoppingCart.Application.DTOs.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.Features.Cart.Requests.Commands
{
	public record CheckoutCart(BasketCheckout BasketCheckout) : IRequest<Unit>;
}
