using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.DTOs.Product
{
    public class ProductCreateDto
    {
        public string name { get; set; }
        public double? price { get; set; }
        public string? description { get; set; }
        public string? categoryName { get; set; }
        public string? imageUrl { get; set; }
    }
}
