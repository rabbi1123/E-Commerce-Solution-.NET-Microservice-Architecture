using ShoppingCart.Application.DTOs.Common;
using ShoppingCart.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Application.DTOs.Cart
{
    public class CartHeaderDto : BaseDto
    {
        public string UserId { get; set; }
        // public string CouponCode { get; set; }
        public double TotalPrice { get; set; }
        public List<CartDetailsDto> CartDetails { get; set; }
    }
}
