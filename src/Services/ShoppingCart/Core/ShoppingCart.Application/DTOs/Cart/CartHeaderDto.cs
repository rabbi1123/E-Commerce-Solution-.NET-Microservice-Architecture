using ShoppingCart.Application.DTOs.Common;

namespace ShoppingCart.Application.DTOs.Cart
{
	public class CartHeaderDto : BaseDto
	{
		public string UserId { get; set; }
		public double TotalPrice { get; set; }
		public List<CartDetailsDto> CartDetails { get; set; }
	}
}
