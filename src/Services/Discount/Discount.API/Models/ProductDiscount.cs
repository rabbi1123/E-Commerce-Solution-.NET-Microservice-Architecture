namespace Discount.API.Models
{
    public class ProductDiscount : BaseDomainEntity
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public double Amount { get; set; }
    }
}
