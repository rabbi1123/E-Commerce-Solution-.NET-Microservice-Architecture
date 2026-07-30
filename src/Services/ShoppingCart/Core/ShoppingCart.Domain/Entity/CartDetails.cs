using ShoppingCart.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Domain.Entity
{
    public class CartDetails : BaseDomainEntity
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public double UnitPrice { get; set; }
        public int CardHeaderId { get; set; }

        [ForeignKey("CardHeaderId")]
        public CartHeader CartHeader { get; set; }
    }
}
