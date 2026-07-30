using Discount.API.Models;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<ProductDiscount> GetDiscount(int productId);
        Task<bool> CreateDiscount(ProductDiscount coupon);
        Task<bool> UpdateDiscount(ProductDiscount coupon);
        Task<bool> DeleteDiscount(int productId);
    }
}
