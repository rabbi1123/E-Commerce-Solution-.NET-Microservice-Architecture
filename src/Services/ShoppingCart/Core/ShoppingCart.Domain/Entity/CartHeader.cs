using ShoppingCart.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Entity
{
    public class CartHeader : BaseDomainEntity
    {
        public string UserId { get; set; }
       // public string? CouponCode { get; set; }
        public double TotalPrice { get; set; }
        public List<CartDetails>? CartDetails { get; set; }
    }
}
