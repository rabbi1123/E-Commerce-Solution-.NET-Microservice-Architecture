using ShoppingCart.Application.DTOs.Common;

namespace ShoppingCart.Application.DTOs.Cart
{
	public class CartDetailsDto : BaseDto
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int Count { get; set; }
		public double UnitPrice { get; set; }
		public int CardHeaderId { get; set; }
	}
}
