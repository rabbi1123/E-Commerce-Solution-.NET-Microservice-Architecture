using ShoppingCart.Application.DTOs.Common;
using ShoppingCart.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
